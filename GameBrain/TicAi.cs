namespace GameBrain;

public class TicAi
{
    public static string PlaceNewPiece(TicTacTwoBrain gameInstance)
    {
        var bestMoves = GetEmptySquares(gameInstance);
        Random newRand = new Random();
        int highestScore = 0;
        foreach (var point in gameInstance.GameGrid.Area)
        {
            if (gameInstance.GameBoard[point.X][point.Y] != EGamePiece.Empty) continue;
            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    int tempScore1 = 0;
                    int tempScore2 = 0;
                    var nextPoint = new Point(point.X + dx, point.Y + dy);
                    for (int i = 0; i < gameInstance.GameConfiguration.WinCondition; i++)
                    {
                        if (!gameInstance.GameGrid.Area.Contains(nextPoint))
                        {
                            break;
                        }

                        if (gameInstance.GameBoard[nextPoint.X][nextPoint.Y] != gameInstance.NextMoveBy.PlSymbol
                            && gameInstance.GameBoard[nextPoint.X][nextPoint.Y] != EGamePiece.Empty)
                        {
                            tempScore2++;
                            if (tempScore2 > highestScore)
                            {
                                bestMoves = new List<Point>();
                                highestScore = tempScore2;
                                bestMoves.Add(point);
                            }
                        }

                        if (gameInstance.GameBoard[nextPoint.X][nextPoint.Y] == gameInstance.NextMoveBy.PlSymbol)
                        {
                            tempScore1++;
                            if (tempScore1 > highestScore)
                            {
                                bestMoves = new List<Point>();
                                highestScore = tempScore1;
                                bestMoves.Add(point);
                            }
                        }
                        
                        else if (tempScore2 == highestScore || tempScore1 == highestScore)
                        {
                            if (!bestMoves.Contains(point))
                            {
                                bestMoves.Add(point);
                            }
                        }

                        nextPoint = new Point(nextPoint.X + dx, nextPoint.Y + dy);
                    }
                }
            }
        }
        var bestMove = bestMoves[newRand.Next(bestMoves.Count)];
        var status = gameInstance.PlaceAPiece(bestMove.X, bestMove.Y);
        if (status != "success")
        {
            throw new ApplicationException($"AI tried to move, but error: {status}");
        }
        return status;
    }

    private static List<Point> GetEmptySquares(TicTacTwoBrain gameInstance)
    {
        var emptySquares = new List<Point>();
        foreach (var point in gameInstance.GameGrid.Area)
        {
            if (gameInstance.GameBoard[point.X][point.Y] == EGamePiece.Empty)
            {
                emptySquares.Add(point);
            }
        }
        if (emptySquares.Count == 0)
        {
            for (var x = 0; x < gameInstance.DimX; x++)
            {
                for (var y = 0; y < gameInstance.DimY; y++)
                {
                    if (gameInstance.GameBoard[x][y] != EGamePiece.Empty) continue;
                    emptySquares.Add(new Point(x, y));
                }
            }
        }
        
        return emptySquares;
    }
}