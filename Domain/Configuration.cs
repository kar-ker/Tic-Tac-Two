using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Configuration
{
    public Guid Id { get; set; }

    [MaxLength(128)] 
    public string ConfigName { get; set; } = default!;
    
    public int BoardSizeWidth { get; set; } = 5;
    
    public int BoardSizeHeight { get; set; } = 5;
    
    public int WinCondition { get; set; } = 3;

    public int GridSide { get; set; } = 3;

    public int GridX { get; set; } = 2;
    
    public int GridY { get; set; } = 2;
    
    public int NumberOfPieces { get; set; } = 4;
    
    public int StartMovingPieceOnTurnN { get; set; } = 4;
    
    public int StartMovingGridOnTurnN { get; set; } = 4;
    
    [MaxLength(128)]
    public string CreatedAtDateTime { get; set; } = default!;
    
    public ICollection<SaveGame>? SaveGames { get; set; }

    public override string ToString() =>
        $"Configuration name: {ConfigName}, " +
        $"Board {BoardSizeWidth}x{BoardSizeHeight}, " +
        $"Grid side length: {GridSide}, " +
        $"Grid centre point: {GridX},{GridY}, " +
        $"How many in a row to win: {WinCondition}, " +
        $"Total number of Player pieces: {NumberOfPieces}, " +
        $"Can relocate a piece after {StartMovingPieceOnTurnN} moves, " +
        $"Can move the grid after {StartMovingGridOnTurnN} moves, " + 
        " Savegames: " + (SaveGames?.Count().ToString() ?? "not joined");
    
}