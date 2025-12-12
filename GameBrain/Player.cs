namespace GameBrain;

public class Player
{
    public string PlName { get; set; }

    public EGamePiece PlSymbol { get; set; }

    public int PlPiecesOnHand { get; set; }
    
    public string Password { get; set; }

    public Player(string plName, EGamePiece plSymbol, int plPiecesOnHand, string password)
    {
        PlName = plName;
        PlSymbol = plSymbol;
        PlPiecesOnHand = plPiecesOnHand;
        Password = password;
    }
    
}