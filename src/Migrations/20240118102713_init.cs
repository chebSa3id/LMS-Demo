using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LMS_Demo.src.Migrations
{
	/// <inheritdoc />
	public partial class init : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
					name: "Courses",
					columns: table => new
					{
						Id = table.Column<int>(type: "integer", nullable: false)
									.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
						Title = table.Column<string>(type: "text", nullable: false),
						Description = table.Column<string>(type: "text", nullable: true),
						Credits = table.Column<int>(type: "integer", nullable: false)
					},
					constraints: table =>
					{
						table.PrimaryKey("PK_Courses", x => x.Id);
					});

			migrationBuilder.CreateTable(
					name: "Roles",
					columns: table => new
					{
						Id = table.Column<int>(type: "integer", nullable: false)
									.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
						Name = table.Column<string>(type: "text", nullable: false)
					},
					constraints: table =>
					{
						table.PrimaryKey("PK_Roles", x => x.Id);
					});

			migrationBuilder.CreateTable(
					name: "Users",
					columns: table => new
					{
						Id = table.Column<int>(type: "integer", nullable: false)
									.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
						FirstName = table.Column<string>(type: "text", nullable: false),
						LastName = table.Column<string>(type: "text", nullable: true),
						Email = table.Column<string>(type: "text", nullable: false),
						Password = table.Column<string>(type: "text", nullable: false),
						RoleId = table.Column<int>(type: "integer", nullable: false)
					},
					constraints: table =>
					{
						table.PrimaryKey("PK_Users", x => x.Id);
						table.ForeignKey(
											name: "FK_Users_Roles_RoleId",
											column: x => x.RoleId,
											principalTable: "Roles",
											principalColumn: "Id",
											onDelete: ReferentialAction.Cascade);
					});

			migrationBuilder.CreateTable(
					name: "Enrollments",
					columns: table => new
					{
						UserId = table.Column<int>(type: "integer", nullable: false),
						CourseId = table.Column<int>(type: "integer", nullable: false),
						Grade = table.Column<int>(type: "integer", nullable: false)
					},
					constraints: table =>
					{
						table.PrimaryKey("PK_Enrollments", x => new { x.UserId, x.CourseId });
						table.ForeignKey(
											name: "FK_Enrollments_Courses_CourseId",
											column: x => x.CourseId,
											principalTable: "Courses",
											principalColumn: "Id",
											onDelete: ReferentialAction.Cascade);
						table.ForeignKey(
											name: "FK_Enrollments_Users_UserId",
											column: x => x.UserId,
											principalTable: "Users",
											principalColumn: "Id",
											onDelete: ReferentialAction.Cascade);
					});

			migrationBuilder.InsertData(
					table: "Roles",
					columns: new[] { "Id", "Name" },
					values: new object[,]
					{
										{ 1, "Admin" },
										{ 2, "Student" },
										{ 3, "Instructor" }
					});

			migrationBuilder.InsertData(
					table: "Users",
					columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "RoleId" },
					values: new object[] { 1, "admin@sys.com", "Admin", "Admin", "admin", 1 });

			migrationBuilder.CreateIndex(
					name: "IX_Enrollments_CourseId",
					table: "Enrollments",
					column: "CourseId");

			migrationBuilder.CreateIndex(
					name: "IX_Users_RoleId",
					table: "Users",
					column: "RoleId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
					name: "Enrollments");

			migrationBuilder.DropTable(
					name: "Courses");

			migrationBuilder.DropTable(
					name: "Users");

			migrationBuilder.DropTable(
					name: "Roles");
		}
	}
}
