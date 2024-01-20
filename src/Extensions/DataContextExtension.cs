namespace LMS_Demo.src.Extensions;

public static class DataContextExtension
{
	public static DataContext SeedDatabase(this DataContext dataContext, ModelBuilder modelBuilder)
	{
		DbSeed.SeedData(modelBuilder);
		return dataContext;
	}

	public static async Task<DataContext> ApplyMigrations(this DataContext dataContext)
	{
		try
		{
			var pendingMigrations = await dataContext.Database.GetPendingMigrationsAsync();

			if (pendingMigrations.Count() > 0)
			{
				Log.Information($"Found {pendingMigrations.Count()} pending migrations.");
				await dataContext.Database.MigrateAsync();
				Log.Information("Migrations applied.");
			}
			else
				Log.Information("No pending migrations found.");

			Log.Information("Migrations check completed.");
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Error preparing existing Database for EntityFramework. Stopping the application. \n {0}", e.Message);
			Environment.Exit(1);
		}

		return dataContext;
	}
}