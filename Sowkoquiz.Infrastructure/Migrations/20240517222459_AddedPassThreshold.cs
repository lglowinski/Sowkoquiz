using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sowkoquiz.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedPassThreshold : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "PassedThreshold",
                table: "QuizzDefinitions",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PassedThreshold",
                table: "QuizzDefinitions");
        }
    }
}
