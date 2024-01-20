namespace LMS_Demo.src.Extensions.Setup;

public static class DIContainer
{
	public static IServiceCollection SetupDIContainer(this IServiceCollection services)
	{
		// add db context (entity framework)
		services.AddDbContext<DataContext>();

		// add custom api middlewares
		services.AddTransient<ExceptionMiddleware>();
		services.AddTransient<LoggerMiddleware>();

		// add repositories
		services.AddScoped<IUserRepo, UserRepo>();
		services.AddScoped<ICourseRepo, CourseRepo>();
		services.AddScoped<IEnrollmentRepo, EnrollmentRepo>();

		// add services


		// add utils

		services.AddAutoMapper(typeof(MappingConfig).Assembly);

		return services;
	}
}