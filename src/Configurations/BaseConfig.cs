namespace LMS_Demo.src.Configurations;
public class BaseConfig
{
	[Required]
	public string AppName { get; set; }
	[Required]
	public string AppSettingsFile { get; set; } // log the appsettings file app is using (for troubleshooting)
	[Required]
	public string HostPort { get; set; } // the port the app will run on
	[Required]
	public string ProjectFolderPath { get; set; } // the path to the root project folder
	[Required]
	public string ConnectionString { get; set; }
}