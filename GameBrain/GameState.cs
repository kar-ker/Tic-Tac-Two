namespace GameBrain;

public class GameState
{
    public EGamePiece[][] GameBoard { get; set; }
    public Player NextMoveBy { get; set; }

    public GameConfiguration GameConfiguration { get; set; }
    
    public Grid GameGrid { get; set; }
    
    public Player Player1 { get; set; }
    public Player Player2 { get; set; }
    
    public EGameMode GameMode { get; set; }
    public int TurnsPlayed { get; set; }
    
    public bool GameOver { get; set; }
    
    public GameState( GameConfiguration gameConfiguration, EGamePiece[][] gameBoard, Grid gameGrid, Player player1, Player player2, EGameMode gameMode)
    {
        GameBoard = gameBoard;
        GameConfiguration = gameConfiguration;
        GameGrid = gameGrid;
        Player1 = player1;
        Player2 = player2;
        GameMode = gameMode;
        NextMoveBy = Player1;
        TurnsPlayed = 0;
        GameOver = false;
    }
    
    public override string ToString()
    {
        return System.Text.Json.JsonSerializer.Serialize(this);
    }
}