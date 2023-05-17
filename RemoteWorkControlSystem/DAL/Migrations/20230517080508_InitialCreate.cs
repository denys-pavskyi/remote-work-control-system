using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectKey = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ProjectTitle = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    JiraDomain = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsScreenActivityControlEnabled = table.Column<bool>(type: "bit", nullable: false),
                    ScreenshotInterval = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JiraBaseUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JiraApiKey = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectMembers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjectMembers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeScreenActivities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScreenshotURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectMemberId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeScreenActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeScreenActivities_ProjectMembers_ProjectMemberId",
                        column: x => x.ProjectMemberId,
                        principalTable: "ProjectMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectMemberId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SprintKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaskKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WorkTime = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkSessions_ProjectMembers_ProjectMemberId",
                        column: x => x.ProjectMemberId,
                        principalTable: "ProjectMembers",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "JiraApiKey", "JiraBaseUrl", "LastName", "Password", "UserName" },
                values: new object[] { 1, "dpavsky@gmail.com", "Denys", "ATATT3xFfGF0zKVExXVUI7se6r5sZekIGQL9cgiwmLiWCgDXjstSgt48rtJhJvX71geSrJbOdWPz1c8I1tqWvSVWdI_gJfoAxDpS8XJYkF_SZG6wcLpV_Eu8c44v7436cgwvuJ63rjh-Zluy7Svvsrg_e6hRm-a83pg6AMyM47qZ9OGzFpeEUJQ=0CD67135", "test-rwcs", "Pavskyi", "password1", "denys_pavskyi2" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "JiraApiKey", "JiraBaseUrl", "LastName", "Password", "UserName" },
                values: new object[] { 2, "denchik.arasty000@gmail.com", "Denis", "ATATT3xFfGF04dWC_ws0K9fPjFB1KIZtP4TSisM-yAKQzQEn6hGqwElrEraynNKfFcz6KVx7Kv1dYIML9CdtqdTfhSAcCJHTDzclxOSrRQ3UUP1KFpOAABfKVYvg6qxd9Y3ni9WBDmTkmtVY56fOvebM0cYh-wiHBtjNwI0rSVNK7rQW9wccXig=6019D62C", "test-rwcs", "Test", "password1", "denis_test" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeScreenActivities_ProjectMemberId",
                table: "EmployeeScreenActivities",
                column: "ProjectMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_ProjectId",
                table: "ProjectMembers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_UserId",
                table: "ProjectMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkSessions_ProjectMemberId",
                table: "WorkSessions",
                column: "ProjectMemberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeScreenActivities");

            migrationBuilder.DropTable(
                name: "WorkSessions");

            migrationBuilder.DropTable(
                name: "ProjectMembers");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
