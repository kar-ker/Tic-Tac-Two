using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class RenamedColumnsInConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MovePieceAfterNTurns",
                table: "Configurations",
                newName: "StartMovingPieceOnTurnN");

            migrationBuilder.RenameColumn(
                name: "MoveGridAfterNTurns",
                table: "Configurations",
                newName: "StartMovingGridOnTurnN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartMovingPieceOnTurnN",
                table: "Configurations",
                newName: "MovePieceAfterNTurns");

            migrationBuilder.RenameColumn(
                name: "StartMovingGridOnTurnN",
                table: "Configurations",
                newName: "MoveGridAfterNTurns");
        }
    }
}
