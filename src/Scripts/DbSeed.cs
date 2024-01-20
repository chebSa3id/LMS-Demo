namespace LMS_Demo.src.Scripts;

public class DbSeed
{
	public static void SeedData(ModelBuilder modelBuilder)
	{
		// Seed Roles
		modelBuilder.Entity<Role>().HasData(
			new Role { Id = 1, Name = "Admin" },
			new Role { Id = 2, Name = "Student" },
			new Role { Id = 3, Name = "Instructor" }
		);
	}
}