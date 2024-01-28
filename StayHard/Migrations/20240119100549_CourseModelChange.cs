using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StayHard.Migrations
{
    /// <inheritdoc />
    public partial class CourseModelChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activation",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Group",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Semester",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activation",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Group",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Semester",
                table: "Courses");
        }
    }
}
