using LMS_Demo.src.Dtos.Common;

namespace LMS_Demo.src.Middlewares;

public class ExceptionMiddleware : IMiddleware
{
	private readonly ILogger<ExceptionMiddleware> _logger;
	public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
	{
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
	{
		// store a reference to the original response body stream (as LoggerMiddleware temporarily re-points the response body to a new memory stream to be able to read the response body)
		var originalResponseBodyReference = httpContext.Response.Body;

		try
		{
			await next(httpContext);
		}
		catch (Exception e)
		{
			_logger.LogError(e, e.Message);
			httpContext.Response.ContentType = "application/json";
			httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

			bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

			var response = isDevelopment
					? new ExceptionDto(httpContext.Response.StatusCode, e.Message, e.StackTrace?.ToString())
					: new ExceptionDto(httpContext.Response.StatusCode, "Internal Server Error");

			var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

			var json = JsonSerializer.Serialize(response, options);

			// re-point the Response.Body back to the original response body stream (as LoggerMiddleware has re-pointed it to a new memory stream to be able to read the response body, but didn't re-point it back to the original response body stream due to the exception being thrown in the controller)
			httpContext.Response.Body = originalResponseBodyReference;

			// write the json to the Response Body stream.
			await httpContext.Response.WriteAsync(json);
		}
	}
}