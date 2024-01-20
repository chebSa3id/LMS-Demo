using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS_Demo.src.Migrations
{
	/// <inheritdoc />
	public partial class adjust_Enrollment_nullables : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DeleteData(
					table: "Users",
					keyColumn: "Id",
					keyValue: 1);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.InsertData(
					table: "Users",
					columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "RoleId" },
					values: new object[] { 1, "admin@sys.com", "Admin", "Admin", "admin", 1 });
		}
	}
}
