namespace LMS_Demo.src.Extensions.Setup;

public static class ConfigurationExtension
{
	public static WebApplicationBuilder SetupConfiguration(this WebApplicationBuilder builder)
	{
		// get configuration values from appsettings (default & gets overwritten by appsettings.{environment}.json)
		builder.Configuration
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
			.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

		// bind appsettings values to configuration classes & add them to DI container (throw validation error if required fields are missing in appsettings file).
		builder.Services.AddOptions<BaseConfig>()
			.BindConfiguration("Base")
			.ValidateDataAnnotations()
			.ValidateOnStart();

		builder.Services.AddOptions<LoggerConfig>()
			.BindConfiguration("Logger")
			.ValidateDataAnnotations()
			.ValidateOnStart();
		return builder;
	}
}