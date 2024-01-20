namespace LMS_Demo.src.Configurations;

public class LoggerConfig
{
	[Required]
	public string StructuredLogUrl { get; set; } // URL to send json logs to (sc-monitor backend)
	[Required]
	public bool WriteToFile { get; set; }
	[Required]
	public bool WriteToConsole { get; set; }
	[Required]
	public bool WriteToStructuredLog { get; set; }
	[Required]
	public string MinimumLogLevel { get; set; }
}