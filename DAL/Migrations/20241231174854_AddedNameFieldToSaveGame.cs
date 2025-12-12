using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedNameFieldToSaveGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Configurations",
                newName: "ConfigName");

            migrationBuilder.AddColumn<string>(
                name: "SaveGameName",
                table: "SaveGames",
                type: "TEXT",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "SaveGameState",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    State = table.Column<string>(type: "TEXT", maxLength: 10240, nullable: false),
                    SaveGameId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaveGameState", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaveGameState_SaveGames_SaveGameId",
                        column: x => x.SaveGameId,
                        principalTable: "SaveGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SaveGameState_SaveGameId",
                table: "SaveGameState",
                column: "SaveGameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaveGameState");

            migrationBuilder.DropColumn(
                name: "SaveGameName",
                table: "SaveGames");

            migrationBuilder.RenameColumn(
                name: "ConfigName",
                table: "Configurations",
                newName: "Name");
        }
    }
}
