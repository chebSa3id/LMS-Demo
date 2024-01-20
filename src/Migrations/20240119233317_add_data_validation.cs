using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS_Demo.src.Migrations
{
	/// <inheritdoc />
	public partial class add_data_validation : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<string>(
					name: "Password",
					table: "Users",
					type: "character varying(100)",
					maxLength: 100,
					nullable: false,
					oldClrType: typeof(string),
					oldType: "text");

			migrationBuilder.AlterColumn<string>(
					name: "LastName",
					table: "Users",
					type: "character varying(50)",
					maxLength: 50,
					nullable: false,
					defaultValue: "",
					oldClrType: typeof(string),
					oldType: "text",
					oldNullable: true);

			migrationBuilder.AlterColumn<string>(
					name: "FirstName",
					table: "Users",
					type: "character varying(50)",
					maxLength: 50,
					nullable: false,
					oldClrType: typeof(string),
					oldType: "text");

			migrationBuilder.AlterColumn<string>(
					name: "Title",
					table: "Courses",
					type: "character varying(100)",
					maxLength: 100,
					nullable: false,
					oldClrType: typeof(string),
					oldType: "text");

			migrationBuilder.AlterColumn<string>(
					name: "Description",
					table: "Courses",
					type: "character varying(500)",
					maxLength: 500,
					nullable: true,
					oldClrType: typeof(string),
					oldType: "text",
					oldNullable: true);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<string>(
					name: "Password",
					table: "Users",
					type: "text",
					nullable: false,
					oldClrType: typeof(string),
					oldType: "character varying(100)",
					oldMaxLength: 100);

			migrationBuilder.AlterColumn<string>(
					name: "LastName",
					table: "Users",
					type: "text",
					nullable: true,
					oldClrType: typeof(string),
					oldType: "character varying(50)",
					oldMaxLength: 50);

			migrationBuilder.AlterColumn<string>(
					name: "FirstName",
					table: "Users",
					type: "text",
					nullable: false,
					oldClrType: typeof(string),
					oldType: "character varying(50)",
					oldMaxLength: 50);

			migrationBuilder.AlterColumn<string>(
					name: "Title",
					table: "Courses",
					type: "text",
					nullable: false,
					oldClrType: typeof(string),
					oldType: "character varying(100)",
					oldMaxLength: 100);

			migrationBuilder.AlterColumn<string>(
					name: "Description",
					table: "Courses",
					type: "text",
					nullable: true,
					oldClrType: typeof(string),
					oldType: "character varying(500)",
					oldMaxLength: 500,
					oldNullable: true);
		}
	}
}
