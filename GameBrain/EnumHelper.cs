namespace GameBrain;

public static class EnumHelper
{
    public static EGameMode GetEGameMode(string gameMode) =>
        gameMode switch
        {
            "PvP" => EGameMode.PvP,
            "PvAi" => EGameMode.PvAi,
            "AivAi" => EGameMode.AivAi,
            _ => throw new Exception($"Unknown game mode string: {gameMode}"),
        };
    
    public static string EGameModeToString(EGameMode gameMode) =>
        gameMode switch
        {
            EGameMode.PvP => "PvP",
            EGameMode.PvAi => "PvAi",
            EGameMode.AivAi => "AivAi" ,
            _ => throw new Exception($"Unknown game mode: {gameMode}"),
        };


    public static EGamePiece GetEGamePiece(string piece) =>
        piece switch
        {
            "X" => EGamePiece.X,
            "O" => EGamePiece.O,
            "" => EGamePiece.Empty,
            _ => throw new Exception($"Unknown game piece string: {piece}"),
        };
    
    public static string EGamePieceToString(EGamePiece piece) =>
        piece switch
        {
            EGamePiece.O => "O",
            EGamePiece.X => "X",
            EGamePiece.Empty => "",
            _ => throw new Exception($"Unknown game piece: {piece}"),
        };
    
    public static string DrawGamePiece(EGamePiece piece) =>// for console UI
        piece switch
        {
            EGamePiece.O => "O",
            EGamePiece.X => "X",
            EGamePiece.Empty => " ",
            _ => throw new Exception($"Unknown game piece: {piece}"),
        };
    
    public static List<EGameMode> GetGameModes() => new List<EGameMode> { EGameMode.PvP, EGameMode.PvAi, EGameMode.AivAi };
    
    
}