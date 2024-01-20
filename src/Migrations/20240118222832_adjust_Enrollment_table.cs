using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS_Demo.src.Migrations
{
	/// <inheritdoc />
	public partial class adjust_Enrollment_table : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<bool>(
					name: "IsPaid",
					table: "Enrollments",
					type: "boolean",
					nullable: false,
					defaultValue: false);

			migrationBuilder.AddColumn<int>(
					name: "ReceiptNumber",
					table: "Enrollments",
					type: "integer",
					nullable: false,
					defaultValue: 0);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
					name: "IsPaid",
					table: "Enrollments");

			migrationBuilder.DropColumn(
					name: "ReceiptNumber",
					table: "Enrollments");
		}
	}
}
