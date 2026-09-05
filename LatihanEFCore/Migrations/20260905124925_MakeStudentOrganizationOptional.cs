using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LatihanEFCore.DTO.Responses.Migrations
{
    /// <inheritdoc />
    public partial class MakeStudentOrganizationOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityPoints_Students_IdStudent",
                table: "ActivityPoints");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Teachers_TeacherId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Students_StudentId",
                table: "Organizations");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Teachers_TeacherId",
                table: "Organizations");

            migrationBuilder.DropForeignKey(
                name: "FK_Tuitions_Courses_CourseId",
                table: "Tuitions");

            migrationBuilder.DropForeignKey(
                name: "FK_Tuitions_Students_StudentId",
                table: "Tuitions");

            migrationBuilder.DropIndex(
                name: "IX_Tuitions_CourseId",
                table: "Tuitions");

            migrationBuilder.DropIndex(
                name: "IX_Tuitions_StudentId",
                table: "Tuitions");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_StudentId",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_TeacherId",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_ActivityPoints_IdStudent",
                table: "ActivityPoints");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Tuitions");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "IdStudent",
                table: "ActivityPoints");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Tuitions",
                newName: "IdStudent");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "Organizations",
                newName: "IdTeacher");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "Courses",
                newName: "IdTeacher");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_TeacherId",
                table: "Courses",
                newName: "IX_Courses_IdTeacher");

            migrationBuilder.AddColumn<string>(
                name: "IdCourse",
                table: "Tuitions",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Teachers",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 20)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "idCourse",
                table: "Teachers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Students",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 20)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "IdOrganization",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ClassroomId",
                table: "Courses",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ClassroomIdClassroom",
                table: "Courses",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "TeacherIdTeacher",
                table: "Courses",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdClassroom",
                table: "Classrooms",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.CreateTable(
                name: "StudentActivityPoints",
                columns: table => new
                {
                    IdStudent = table.Column<int>(type: "int", nullable: false),
                    IdActivityPoints = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentActivityPoints", x => new { x.IdStudent, x.IdActivityPoints });
                    table.ForeignKey(
                        name: "FK_StudentActivityPoints_ActivityPoints_IdActivityPoints",
                        column: x => x.IdActivityPoints,
                        principalTable: "ActivityPoints",
                        principalColumn: "IdActivityPoints",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentActivityPoints_Students_IdStudent",
                        column: x => x.IdStudent,
                        principalTable: "Students",
                        principalColumn: "IdStudent",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StudentCourses",
                columns: table => new
                {
                    IdStudent = table.Column<int>(type: "int", nullable: false),
                    IdCourse = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentCourses", x => new { x.IdStudent, x.IdCourse });
                    table.ForeignKey(
                        name: "FK_StudentCourses_Courses_IdCourse",
                        column: x => x.IdCourse,
                        principalTable: "Courses",
                        principalColumn: "IdCourse",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentCourses_Students_IdStudent",
                        column: x => x.IdStudent,
                        principalTable: "Students",
                        principalColumn: "IdStudent",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Tuitions_IdCourse",
                table: "Tuitions",
                column: "IdCourse");

            migrationBuilder.CreateIndex(
                name: "IX_Tuitions_IdStudent",
                table: "Tuitions",
                column: "IdStudent");

            migrationBuilder.CreateIndex(
                name: "IX_Students_IdOrganization",
                table: "Students",
                column: "IdOrganization");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_IdTeacher",
                table: "Organizations",
                column: "IdTeacher",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_ClassroomId",
                table: "Courses",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_ClassroomIdClassroom",
                table: "Courses",
                column: "ClassroomIdClassroom");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TeacherIdTeacher",
                table: "Courses",
                column: "TeacherIdTeacher");

            migrationBuilder.CreateIndex(
                name: "IX_StudentActivityPoints_IdActivityPoints",
                table: "StudentActivityPoints",
                column: "IdActivityPoints");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourses_IdCourse",
                table: "StudentCourses",
                column: "IdCourse");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Classrooms_ClassroomId",
                table: "Courses",
                column: "ClassroomId",
                principalTable: "Classrooms",
                principalColumn: "IdClassroom",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Classrooms_ClassroomIdClassroom",
                table: "Courses",
                column: "ClassroomIdClassroom",
                principalTable: "Classrooms",
                principalColumn: "IdClassroom");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Teachers_IdTeacher",
                table: "Courses",
                column: "IdTeacher",
                principalTable: "Teachers",
                principalColumn: "IdTeacher",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Teachers_TeacherIdTeacher",
                table: "Courses",
                column: "TeacherIdTeacher",
                principalTable: "Teachers",
                principalColumn: "IdTeacher");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Teachers_IdTeacher",
                table: "Organizations",
                column: "IdTeacher",
                principalTable: "Teachers",
                principalColumn: "IdTeacher",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Organizations_IdOrganization",
                table: "Students",
                column: "IdOrganization",
                principalTable: "Organizations",
                principalColumn: "IdOrganization",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tuitions_Courses_IdCourse",
                table: "Tuitions",
                column: "IdCourse",
                principalTable: "Courses",
                principalColumn: "IdCourse",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tuitions_Students_IdStudent",
                table: "Tuitions",
                column: "IdStudent",
                principalTable: "Students",
                principalColumn: "IdStudent",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Classrooms_ClassroomId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Classrooms_ClassroomIdClassroom",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Teachers_IdTeacher",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Teachers_TeacherIdTeacher",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Teachers_IdTeacher",
                table: "Organizations");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Organizations_IdOrganization",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Tuitions_Courses_IdCourse",
                table: "Tuitions");

            migrationBuilder.DropForeignKey(
                name: "FK_Tuitions_Students_IdStudent",
                table: "Tuitions");

            migrationBuilder.DropTable(
                name: "StudentActivityPoints");

            migrationBuilder.DropTable(
                name: "StudentCourses");

            migrationBuilder.DropIndex(
                name: "IX_Tuitions_IdCourse",
                table: "Tuitions");

            migrationBuilder.DropIndex(
                name: "IX_Tuitions_IdStudent",
                table: "Tuitions");

            migrationBuilder.DropIndex(
                name: "IX_Students_IdOrganization",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_IdTeacher",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Courses_ClassroomId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_ClassroomIdClassroom",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_TeacherIdTeacher",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "IdCourse",
                table: "Tuitions");

            migrationBuilder.DropColumn(
                name: "idCourse",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "IdOrganization",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ClassroomId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "ClassroomIdClassroom",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "TeacherIdTeacher",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "IdStudent",
                table: "Tuitions",
                newName: "StudentId");

            migrationBuilder.RenameColumn(
                name: "IdTeacher",
                table: "Organizations",
                newName: "TeacherId");

            migrationBuilder.RenameColumn(
                name: "IdTeacher",
                table: "Courses",
                newName: "TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_IdTeacher",
                table: "Courses",
                newName: "IX_Courses_TeacherId");

            migrationBuilder.AddColumn<string>(
                name: "CourseId",
                table: "Tuitions",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "PhoneNumber",
                table: "Teachers",
                type: "int",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "PhoneNumber",
                table: "Students",
                type: "int",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "Organizations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "IdClassroom",
                table: "Classrooms",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "IdStudent",
                table: "ActivityPoints",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tuitions_CourseId",
                table: "Tuitions",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Tuitions_StudentId",
                table: "Tuitions",
                column: "StudentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_StudentId",
                table: "Organizations",
                column: "StudentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_TeacherId",
                table: "Organizations",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityPoints_IdStudent",
                table: "ActivityPoints",
                column: "IdStudent",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityPoints_Students_IdStudent",
                table: "ActivityPoints",
                column: "IdStudent",
                principalTable: "Students",
                principalColumn: "IdStudent",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Teachers_TeacherId",
                table: "Courses",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "IdTeacher",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Students_StudentId",
                table: "Organizations",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "IdStudent",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Teachers_TeacherId",
                table: "Organizations",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "IdTeacher",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tuitions_Courses_CourseId",
                table: "Tuitions",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "IdCourse",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tuitions_Students_StudentId",
                table: "Tuitions",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "IdStudent",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
