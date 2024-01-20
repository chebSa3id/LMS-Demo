using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS_Demo.src.Migrations
{
	/// <inheritdoc />
	public partial class adjust_Enrollment_IsEnrolled : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<bool>(
					name: "IsEnrolled",
					table: "Enrollments",
					type: "boolean",
					nullable: false,
					defaultValue: false);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
					name: "IsEnrolled",
					table: "Enrollments");
		}
	}
}
