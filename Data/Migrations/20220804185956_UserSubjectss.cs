using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Data.Migrations
{
    public partial class UserSubjectss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subject_AspNetUsers_UserId",
                table: "Subject");

            migrationBuilder.DropIndex(
                name: "IX_Subject_UserId",
                table: "Subject");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Subject");

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserSubjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSubjects_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSubjects_Subject_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SubjectId",
                table: "AspNetUsers",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubjects_SubjectId",
                table: "UserSubjects",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubjects_UserId",
                table: "UserSubjects",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Subject_SubjectId",
                table: "AspNetUsers",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Subject_SubjectId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "UserSubjects");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SubjectId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Subject",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subject_UserId",
                table: "Subject",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subject_AspNetUsers_UserId",
                table: "Subject",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
