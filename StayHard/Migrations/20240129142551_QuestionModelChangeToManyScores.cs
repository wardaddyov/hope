using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StayHard.Migrations
{
    /// <inheritdoc />
    public partial class QuestionModelChangeToManyScores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Scores_QuestionId",
                table: "Scores");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_QuestionId",
                table: "Scores",
                column: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Scores_QuestionId",
                table: "Scores");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_QuestionId",
                table: "Scores",
                column: "QuestionId",
                unique: true);
        }
    }
}
