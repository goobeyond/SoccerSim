using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoccerSim.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Standings_GroupId",
                table: "Standings");

            migrationBuilder.CreateIndex(
                name: "IX_Standings_GroupId_TeamName",
                table: "Standings",
                columns: new[] { "GroupId", "TeamName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Standings_GroupId_TeamName",
                table: "Standings");

            migrationBuilder.CreateIndex(
                name: "IX_Standings_GroupId",
                table: "Standings",
                column: "GroupId");
        }
    }
}
