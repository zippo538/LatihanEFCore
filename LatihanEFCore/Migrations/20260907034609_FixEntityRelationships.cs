using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LatihanEFCore.DTO.Responses.Migrations
{
    /// <inheritdoc />
    public partial class FixEntityRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Teachers_TeacherIdTeacher",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_TeacherIdTeacher",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "TeacherIdTeacher",
                table: "Courses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeacherIdTeacher",
                table: "Courses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TeacherIdTeacher",
                table: "Courses",
                column: "TeacherIdTeacher");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Teachers_TeacherIdTeacher",
                table: "Courses",
                column: "TeacherIdTeacher",
                principalTable: "Teachers",
                principalColumn: "IdTeacher");
        }
    }
}
