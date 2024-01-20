using Figgle;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;

namespace LMS_Demo.src.Extensions.SetupExtensions;

public static class LoggerExtension
{
	public static void SetupLogger(this WebApplicationBuilder builder)
	{
		_ = builder.Host.UseSerilog((_, sp, serilogConfig) =>
				{
					// use 'IOptionsMonitor' (instead of injecting it via DI so config values get reloaded without restarting app).
					var baseConfig = sp.GetRequiredService<IOptionsMonitor<BaseConfig>>().CurrentValue;
					var loggerConfigs = sp.GetRequiredService<IOptionsMonitor<LoggerConfig>>().CurrentValue;

					bool writeToStructuredLog = loggerConfigs.WriteToStructuredLog;

					string formattedAppName = baseConfig.AppName?.ToLower().Replace(".", "-").Replace(" ", "-");

					string logFilePath = $"{baseConfig.ProjectFolderPath}/Logs - {formattedAppName}/{formattedAppName}";

					ConfigureEnrichers(serilogConfig, baseConfig.AppName);
					ConfigureConsoleLogging(serilogConfig, loggerConfigs.WriteToConsole);
					ConfigureWriteToFile(serilogConfig, loggerConfigs.WriteToFile, logFilePath);
					ConfigureStructuredLog(builder, serilogConfig, baseConfig.AppName, loggerConfigs.WriteToStructuredLog, logFilePath);
					SetMinimumLogLevel(serilogConfig, loggerConfigs.MinimumLogLevel);
					Console.WriteLine(FiggleFonts.Standard.Render(baseConfig.AppName));
				});
	}

	private static void ConfigureEnrichers(LoggerConfiguration serilogConfig, string appName)
	{
		// enrichers are used to add additional information to logs.
		serilogConfig
			.Enrich.FromLogContext()
			.Enrich.WithProperty("Application", appName)
			.Enrich.WithExceptionDetails()
			.Enrich.WithMachineName()
			// .Enrich.WithProcessId()
			.Enrich.WithThreadId()
			.Enrich.FromLogContext();
	}

	private static void ConfigureConsoleLogging(LoggerConfiguration serilogConfig, bool writeToConsole)
	{
		if (writeToConsole)
			serilogConfig.WriteTo.Async(wt => wt.Console());
	}

	private static void ConfigureWriteToFile(LoggerConfiguration serilogConfig, bool writeToFile, string logFilePath)
	{
		if (writeToFile)
		{
			serilogConfig.WriteTo.File(
			 $"{logFilePath}_.txt",
			 rollingInterval: RollingInterval.Day,
			 outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] <Thread:{ThreadId}> {Message:l}{NewLine}{Exception}");
		}
	}

	private static void ConfigureStructuredLog(WebApplicationBuilder builder, LoggerConfiguration serilogConfig, string appName, bool writeToStructuredLog, string logFilePath)
	{
		if (writeToStructuredLog)
		{
			// configure structured log to write to json file (which will then be sent to sc-monitor backend).
			serilogConfig.WriteTo.Async(writeTo =>
				writeTo.File(
					// prefer 'CompactJsonFormatter' (more concise --> ideal for storage & transmitting over network).
					// alternative 'JsonFormatter' (more human readable --> don't need as we already have text logs).
					// NOTE: timestamp in 'JsonFormatter' is based on machine time NOT UTC time (although it is in ISO format!)
					new CompactJsonFormatter(),
					$"{logFilePath}_.json",
					rollingInterval: RollingInterval.Day))
			.Enrich.WithProperty("Environment", builder.Environment.EnvironmentName!);
		}
	}

	private static void SetMinimumLogLevel(LoggerConfiguration serilogConfig, string minLogLevel)
	{
		switch (minLogLevel.ToLower())
		{
			case "verbose":
				serilogConfig.MinimumLevel.Verbose();
				break;
			case "debug":
				serilogConfig.MinimumLevel.Debug();
				break;
			case "information":
				serilogConfig.MinimumLevel.Information();
				break;
			case "warning":
				serilogConfig.MinimumLevel.Warning();
				break;
			case "error":
				serilogConfig.MinimumLevel.Error();
				break;
			case "fatal":
				serilogConfig.MinimumLevel.Fatal();
				break;
			default:
				serilogConfig.MinimumLevel.Information();
				break;
		}
	}
}