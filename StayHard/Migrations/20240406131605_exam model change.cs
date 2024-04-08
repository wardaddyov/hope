using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StayHard.Migrations
{
    /// <inheritdoc />
    public partial class exammodelchange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnswerFileId",
                table: "Exams",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AvailableScore",
                table: "Exams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Exams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "FileAccessible",
                table: "Exams",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsExercise",
                table: "Exams",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOpenBook",
                table: "Exams",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "QuestionFileId",
                table: "Exams",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Exams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "ExamParticipants",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExamFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    File = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamFiles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exams_AnswerFileId",
                table: "Exams",
                column: "AnswerFileId",
                unique: true,
                filter: "[AnswerFileId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_QuestionFileId",
                table: "Exams",
                column: "QuestionFileId",
                unique: true,
                filter: "[QuestionFileId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_ExamFiles_AnswerFileId",
                table: "Exams",
                column: "AnswerFileId",
                principalTable: "ExamFiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_ExamFiles_QuestionFileId",
                table: "Exams",
                column: "QuestionFileId",
                principalTable: "ExamFiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_ExamFiles_AnswerFileId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_ExamFiles_QuestionFileId",
                table: "Exams");

            migrationBuilder.DropTable(
                name: "ExamFiles");

            migrationBuilder.DropIndex(
                name: "IX_Exams_AnswerFileId",
                table: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_Exams_QuestionFileId",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "AnswerFileId",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "AvailableScore",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "FileAccessible",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "IsExercise",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "IsOpenBook",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "QuestionFileId",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "ExamParticipants");
        }
    }
}
