namespace LMS_Demo.src.Models;

public partial class DataContext : DbContext
{
	private readonly IOptions<BaseConfig> _baseConfig;

	public DataContext(DbContextOptions<DataContext> options, IOptions<BaseConfig> baseConfig) : base(options)
	{
		_baseConfig = baseConfig;

		// set Db command timeout to 5 minutes.
		// Database.SetCommandTimeout(5 * 60);
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured)
			optionsBuilder.UseNpgsql(_baseConfig.Value.ConnectionString);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// EXAMPLE: overwrite default EF code-first table names (plural to singular)
		// modelBuilder.Entity<EntityToOverwriteTableNameFor>().ToTable("EntityToOverwriteTableNameFor");

		// EXAMPLE: prevent EF from creating a table for class (used for mapping query results to class objects eg. View Tables)
		// modelBuilder.Entity<ViewEntity>().HasNoKey().ToView("ViewEntitys");

		// seed data (use custom DataContext extension method)
		this.SeedDatabase(modelBuilder);
	}

	// define the tables in the database
	public DbSet<User> Users { get; set; }
	public DbSet<Course> Courses { get; set; }
	public DbSet<Enrollment> Enrollments { get; set; }
	public DbSet<Role> Roles { get; set; }


	// EXAMPLE: below are Entities only used to map query results to class objects without creating a table for them (eg. View Tables)
	// NOTE: add .HasNoKey().ToView() in 'OnModelCreating' above to prevent EF from creating a table for class
	// public virtual DbSet<ViewEntity> ViewEntitys { get; set; }
}