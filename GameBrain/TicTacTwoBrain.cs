namespace GameBrain;

public class TicTacTwoBrain
{
    private readonly GameState _gameState;
    
    public TicTacTwoBrain(GameConfiguration gConf, Player player1, Player player2, EGameMode gameMode)
    {
        var gameBoard = new EGamePiece[gConf.BoardSizeWidth][];
        for (int i = 0; i < gameBoard.Length; i++)
        {
            gameBoard[i] = new EGamePiece[gConf.BoardSizeHeight];
        }

        var gameGrid = new Grid(gConf.GridSide, gConf.GridX, gConf.GridY);

        _gameState = new GameState(gConf, gameBoard, gameGrid, player1, player2, gameMode);
    }

    public TicTacTwoBrain(GameState gameState)
    {
        _gameState = gameState;
    }

    public string GetGameStateJson()
    {
        return _gameState.ToString();
    }

    public string GetGameConfigName()
    {
        return _gameState.GameConfiguration.Name;
    }

    public GameConfiguration GameConfiguration => _gameState.GameConfiguration;

    public EGamePiece[][] GameBoard
    {
        get => GetBoard();
        private set => _gameState.GameBoard = value;
    }

    public int DimX => _gameState.GameBoard.Length;
    public int DimY => _gameState.GameBoard[0].Length;

    private EGamePiece[][] GetBoard()
    {
        var copyOfBoard = new EGamePiece[DimX][];
        for (int x = 0; x < DimX; x++)
        {
            var column = new EGamePiece[DimY];
            for (int y = 0; y < DimY; y++)
            {
                column[y] = _gameState.GameBoard[x][y];
            }

            copyOfBoard[x] = column;
        }

        return copyOfBoard;
    }

    public Grid GameGrid => _gameState.GameGrid;

    public int TurnsPlayed => _gameState.TurnsPlayed;

    public Player NextMoveBy
    {
        get => _gameState.NextMoveBy;
        set => _gameState.NextMoveBy = value;
    } 

    public List<Player> Players => [_gameState.Player1, _gameState.Player2];
    public EGameMode GameMode => _gameState.GameMode;

    public bool GameOver
    {
        get => _gameState.GameOver;
        set => _gameState.GameOver = value;
    }

    public string RelocateAPiece(int x1, int y1, int x2, int y2)
    {
        if (_gameState.GameOver)
        {
            return "Game over";
        }
        if (x1 >= DimX || y1 >= DimY || x2 >= DimX || y2 >= DimY
            || x1 < 0 || y1 < 0 || x2 < 0 || y2 < 0)
        {
            Console.WriteLine("Invalid coordinates, off the board.");
            return "Invalid coordinates, off the board.";
        }

        if (_gameState.GameBoard[x1][y1] != _gameState.NextMoveBy.PlSymbol
            || _gameState.GameBoard[x2][y2] != EGamePiece.Empty)
        {
            Console.WriteLine("Illegal move!!!");
            return "Illegal move!!!";
        }

        PickUpPiece(x1, y1);
        PlaceAPiece(x2, y2);
        return "success";
    }

    private void PickUpPiece(int x1, int y1)
    {
        EqualizeObjects();
        _gameState.NextMoveBy.PlPiecesOnHand += 1;
        _gameState.GameBoard[x1][y1] = EGamePiece.Empty;
    }

    public string PlaceAPiece(int x, int y)
    {
        if (_gameState.GameOver)
        {
            return "Game over";
        }

        if (_gameState.NextMoveBy.PlPiecesOnHand < 1)
        {
            Console.WriteLine("No more pieces to place.");
            return "No more pieces to place.";
        }
        
        if (x >= DimX || y >= DimY || x < 0 || y < 0)
        {
            Console.WriteLine("Invalid coordinates, off the board.");
            return "Invalid coordinates, off the board.";
        }

        if (_gameState.GameBoard[x][y] != EGamePiece.Empty)
        {
            Console.WriteLine("Invalid coordinates, There's already a piece there.");
            return "Invalid coordinates, There's already a piece there.";
        }

        EqualizeObjects();
        _gameState.GameBoard[x][y] = _gameState.NextMoveBy.PlSymbol;
        _gameState.NextMoveBy.PlPiecesOnHand -= 1;
        EndTurn();
        return "success";
    }
    
    public string RelocateTheGridWithOffset(int x, int y)
    {
        if (_gameState.GameOver)
        {
            return "Game over";
        }
        
        var grid = _gameState.GameGrid;
        var newArea = new List<Point>();
        
        foreach (var point in grid.Area)
        {
            var newX = point.X + x;
            var newY = point.Y + y;
            if (newX > DimX - 1 || newY > DimY - 1 || newX < 0 || newY < 0)
            {
                Console.WriteLine("Invalid offset, grid will go off the board.");
                return "Invalid offset, grid will go off the board.";
            }
            newArea.Add(new Point(point.X + x, point.Y + y));
        }

        _gameState.GameGrid.Area = newArea;
        EndTurn();
        return "success";
    }
    
    public string RelocateTheGrid(int x, int y)
    {
        if (_gameState.GameOver)
        {
            return "Game over";
        }
        if (x >= DimX || y >= DimY || x < 0 || y < 0)
        {
            Console.WriteLine("Invalid coordinates, off the board.");
            return "Invalid coordinates, off the board.";
        }

        var grid = _gameState.GameGrid;
        var diff = grid.GridSide / 2;
        var even = (grid.GridSide % 2) == 0;
        if (grid.GridX == x && grid.GridY == y)
        {
            Console.WriteLine("You can't relocate the grid to where it already is.");
            return "You can't relocate the grid to where it already is.";
        }

        if (even && (x - diff < 0
                     || x + diff > DimX
                     || y - diff < 0
                     || y + diff > DimY))
        {
            Console.WriteLine("Invalid grid coordinates. Grid would go off the board.");
            return "Invalid grid coordinates. Grid would go off the board.";
        }
        
        if (!even && (x - diff < 0
                      || x + diff > DimX - 1
                      || y - diff < 0
                      || y + diff > DimY - 1))
        {
            Console.WriteLine("Invalid grid coordinates. Grid would go off the board.");
            return "Invalid grid coordinates. Grid would go off the board.";
        }

        if (Math.Abs(grid.GridX - x) > 1 || Math.Abs(grid.GridY - y) > 1)
        {
            Console.WriteLine("Invalid grid coordinates. Grid can only be moved to an adjacent square.");
            return "Invalid grid coordinates. Grid can only be moved to an adjacent square.";
        }

        _gameState.GameGrid = new Grid(_gameState.GameConfiguration.GridSide, x, y);
        EndTurn();
        return "success";
    }

    private void EndTurn()
    {
        _gameState.TurnsPlayed++;
        var winner = ValidateWin();
        if (winner.PlSymbol != EGamePiece.Empty)
        {
            _gameState.GameOver = true;
            return;
        }

        ChangeNextMoveBy();
    }

    private void EqualizeObjects() // Make objects reference the same place
    {
        _gameState.NextMoveBy =
            _gameState.NextMoveBy.PlSymbol == _gameState.Player1.PlSymbol ? _gameState.Player1 : _gameState.Player2;
    }

    private void ChangeNextMoveBy()
    {
        _gameState.NextMoveBy =
            _gameState.NextMoveBy.PlSymbol == _gameState.Player1.PlSymbol ? _gameState.Player2 : _gameState.Player1;
    }

    public Player ValidateWin()
    {
        Player winningPlayer = new Player("Empty", EGamePiece.Empty, 0, "");
        var requiredPiecesOnBoard =
            _gameState.GameConfiguration.NumberOfPieces - _gameState.GameConfiguration.WinCondition;
        if (_gameState.Player1.PlPiecesOnHand > requiredPiecesOnBoard &&
            _gameState.Player2.PlPiecesOnHand > requiredPiecesOnBoard)
        {
            return winningPlayer;
        }

        if (PLayerMeetsWinCondition(NextMoveBy))
        {
            winningPlayer = NextMoveBy;
        }
        else
        {
            ChangeNextMoveBy();
            if (PLayerMeetsWinCondition(NextMoveBy))
            {
                winningPlayer = NextMoveBy;
                return winningPlayer;
            }

            ChangeNextMoveBy();
        }

        return winningPlayer;
    }

    private bool PLayerMeetsWinCondition(Player player)
    {
        List<Point> checkedPoints = new List<Point>();
        foreach (var point in GameGrid.Area)
        {
            checkedPoints.Add(point);
            if (GameBoard[point.X][point.Y] != player.PlSymbol)
            {
                continue;
            }

            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    var nextPoint = new Point(point.X + dx, point.Y + dy);
                    for (int i = 1; i < _gameState.GameConfiguration.WinCondition; i++)
                    {
                        if (checkedPoints.Contains(nextPoint)
                            || !GameGrid.Area.Contains(nextPoint)
                            || GameBoard[nextPoint.X][nextPoint.Y] != player.PlSymbol)
                        {
                            break;
                        }

                        if (i == _gameState.GameConfiguration.WinCondition - 1)
                        {
                            return true;
                        }

                        nextPoint = new Point(nextPoint.X + dx, nextPoint.Y + dy);
                    }
                }
            }
        }

        return false;
    }
}