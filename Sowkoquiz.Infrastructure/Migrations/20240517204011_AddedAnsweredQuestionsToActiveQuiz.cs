using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sowkoquiz.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedAnsweredQuestionsToActiveQuiz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnsweredQuestionsId",
                table: "ActiveQuizzes");

            migrationBuilder.CreateTable(
                name: "AnsweredQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Letter = table.Column<char>(type: "TEXT", nullable: false),
                    ActiveQuizId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnsweredQuestion", x => new { x.Id, x.ActiveQuizId, x.Letter });
                    table.ForeignKey(
                        name: "FK_AnsweredQuestion_ActiveQuizzes_ActiveQuizId",
                        column: x => x.ActiveQuizId,
                        principalTable: "ActiveQuizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnsweredQuestion_ActiveQuizId",
                table: "AnsweredQuestion",
                column: "ActiveQuizId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnsweredQuestion");

            migrationBuilder.AddColumn<string>(
                name: "AnsweredQuestionsId",
                table: "ActiveQuizzes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
