using System.Diagnostics;

namespace LMS_Demo.src.Middlewares;

public class LoggerMiddleware : IMiddleware
{
	private readonly ILogger<LoggerMiddleware> _logger;

	public LoggerMiddleware(ILogger<LoggerMiddleware> logger)
	{
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
	{
		bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

		var watch = Stopwatch.StartNew();

		if (isDevelopment) // if in Dev mode, log request/response body
		{
			// read the body from the stream
			var bodyReader = new StreamReader(httpContext.Request.Body);
			var bodyAsText = await bodyReader.ReadToEndAsync();

			// push the request header and body to the log context
			LogContext.PushProperty("RequestHeaders", httpContext.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true);

			// log only the first 1000 characters of the request body
			LogContext.PushProperty("RequestBody", bodyAsText.Substring(0, bodyAsText.Length > 1000 ? 1000 : bodyAsText?.Length ?? 0));

			// convert the request body back to a stream (so the next MW in the pipeline also has access to the body stream)
			var bytesToWrite = Encoding.ASCII.GetBytes(bodyAsText ?? "");
			var injectedRequestStream = new MemoryStream();
			injectedRequestStream.Write(bytesToWrite, 0, bytesToWrite.Length);
			injectedRequestStream.Seek(0, SeekOrigin.Begin);
			httpContext.Request.Body = injectedRequestStream;

			/*
      Why do this workaround instead of just reading directly from the Response.Body stream?
        - because Response.Body is a forward-only stream and doesn't support seeking or reading the stream a second time.
        - it's like that to make the default configuration of request handling as lightweight & performant as possible.
        - also Response.Body stream has CanRead = false, so can't read from it directly.        
      
      CAUTION by doing this workaround:
        - we're reading the entire response body into memory, which can cause performance issues.
        - we're changing the lightweight & performant default Response.Body stream to a less performant stream.
        (2x points above are not an issue cause we only log response body in development)
        - Caused an issue in ExceptionMiddleware:
          when an exception is thrown in the controller, the Response.Body stream is closed due to the using-statement (so we had to keep a copy of the original Response.Body stream in the ExceptionMiddleware to be able to write to it there).
      */

			using (var tempResponseBodyMemoryStream = new MemoryStream())
			{
				// STEP 1: hold a reference to the original response body stream.
				var originalResponseBodyReference = httpContext.Response.Body;
				// STEP 2: re-point the Response.Body to a new tempResponseBodyMemoryStream (which has CanRead = true).
				httpContext.Response.Body = tempResponseBodyMemoryStream;

				// STEP 3: let the next MW in the pipeline (controller) handle the request & write it's response to the tempResponseBodyMemoryStream.
				await next(httpContext);

				// STEP 4: read the controller's response from the tempResponseBodyMemoryStream (to be logged).
				httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
				var responseBody = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();

				// log only the first 1000 characters of the response body
				LogContext.PushProperty("ResponseBody", responseBody.Substring(0, responseBody.Length > 1000 ? 1000 : responseBody?.Length ?? 0));

				httpContext.Response.Body.Seek(0, SeekOrigin.Begin);

				// STEP 5: copy the contents of the tempResponseBodyMemoryStream (which contains the controller's response) to the original stream, which is then returned to the client.
				await tempResponseBodyMemoryStream.CopyToAsync(originalResponseBodyReference);

				// --> this way, the next MW in the pipeline can have access to the Response.Body.
			}
		}
		else
			await next(httpContext);

		// push the user name and id to the log context
		LogContext.PushProperty("UserId", httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
		LogContext.PushProperty("UserName", httpContext.User.FindFirstValue(ClaimTypes.Name));

		LogContext.PushProperty("RequestMethod", httpContext.Request.Method);
		LogContext.PushProperty("RequestPath", httpContext.Request.Path);

		LogContext.PushProperty("StatusCode", httpContext.Response.StatusCode);

		watch.Stop();

		LogContext.PushProperty("RequestDuration", watch.ElapsedMilliseconds);

		_logger.LogInformation("Request Duration: {RequestDuration}ms - HTTP {RequestMethod} '{RequestPath}' by {UserName}:{UserId} - Headers {RequestHeaders} - Request Body {RequestBody} \nResponded {StatusCode} - Response Body {ResponseBody}");
	}
}