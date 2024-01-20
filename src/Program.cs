// Initialize Serilog logger, and configure the sinks (to be used during startup only).

using LMS_Demo.src.Extensions.SetupExtensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Swashbuckle.AspNetCore.SwaggerGen;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

try
{
	Log.Information("LMS-Demo starting.");

	var builder = WebApplication.CreateBuilder(args);

	builder.Services.AddAuthentication(x =>
	{
		x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
	}).AddJwtBearer(x =>
{
	x.SaveToken = true;
	x.RequireHttpsMetadata = false;
	x.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidAudience = builder.Configuration["JwtSettings:Audience"],
		ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
	};
});

	builder.Services.AddAuthorization();

	// Run custom setup methods
	builder.SetupConfiguration();
	builder.SetupLogger();
	builder.Services.SetupDIContainer();

	// Set port to listen on
	builder.WebHost.UseUrls($"http://*:{builder.Configuration["Base:HostPort"]}");

	builder.Services.AddControllers();

	// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddSwaggerGen();
	builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

	// Add health checks
	builder.Services.AddHealthChecks();

	// NOTE: logger gets configured here, so logs before '.Build()' will only be written to the console!
	var app = builder.Build();

	// middleware used to set the base url for our api (for reverse proxy) - this way the api will respond to requests starting with 'BaseUrl' without having to prefix the 'BaseUrl' to each controller route. NOTE: api endpoints will respond to requests with & without the 'BaseUrl' prefix (as this mw removes the 'BaseUrl' from the request path before handing the request to the next mw)! 
	app.UsePathBase(builder.Configuration["Base:BaseUrl"]);

	// add health checks for both direct hit (works with/without 'BaseUrl' thanks to previous mw).
	app.UseHealthChecks("/health");

	// Check if the database is available & apply migrations
	using (var scope = app.Services.CreateScope())
	{
		var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
		dataContext.ApplyMigrations().Wait();
		//dataContext.SyncStoredProcedures().Wait();
	}

	app.UseMiddleware<ExceptionMiddleware>();

	// logging middleware should come after authentication middleware to gain access to the token claims.
	app.UseMiddleware<LoggerMiddleware>();

	if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI();
	}

	app.UseHttpsRedirection();

	app.UseAuthentication();

	app.UseAuthorization();

	app.MapControllers();

	// log the appsettings file being used (for troubleshooting).
	Log.Information($"Appsettings file: {builder.Configuration["Base:AppSettingsFile"]}");

	await app.RunAsync();
}
catch (Exception ex)
{
	// app failed to start.
	Log.Fatal(ex, "Host terminated unexpectedly");
	return 1;
}
finally
{
	// flush all logs to the log sinks (output destinations) & release used resources before app terminates.
	// if you omit this call, there is a risk that some log events may not be written, and resources may not be released, which can lead to incomplete or lost log data.
	Log.CloseAndFlush();
}

return 0;