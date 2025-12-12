using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedSaveGameStates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaveGameState_SaveGames_SaveGameId",
                table: "SaveGameState");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SaveGameState",
                table: "SaveGameState");

            migrationBuilder.RenameTable(
                name: "SaveGameState",
                newName: "SaveGameStates");

            migrationBuilder.RenameIndex(
                name: "IX_SaveGameState_SaveGameId",
                table: "SaveGameStates",
                newName: "IX_SaveGameStates_SaveGameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SaveGameStates",
                table: "SaveGameStates",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SaveGameStates_SaveGames_SaveGameId",
                table: "SaveGameStates",
                column: "SaveGameId",
                principalTable: "SaveGames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaveGameStates_SaveGames_SaveGameId",
                table: "SaveGameStates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SaveGameStates",
                table: "SaveGameStates");

            migrationBuilder.RenameTable(
                name: "SaveGameStates",
                newName: "SaveGameState");

            migrationBuilder.RenameIndex(
                name: "IX_SaveGameStates_SaveGameId",
                table: "SaveGameState",
                newName: "IX_SaveGameState_SaveGameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SaveGameState",
                table: "SaveGameState",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SaveGameState_SaveGames_SaveGameId",
                table: "SaveGameState",
                column: "SaveGameId",
                principalTable: "SaveGames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
