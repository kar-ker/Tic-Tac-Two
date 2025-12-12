using GameBrain;

namespace ConsoleUI;

public static class Visualizer
{
    public static void DrawBoard(TicTacTwoBrain gameInstance)
    {
        Console.Write(" Y X");
        for (var x = 0; x < gameInstance.DimX; x++)
        {
            Console.Write(" " + x + " " + " ");
        }

        Console.WriteLine();
        for (var y = 0; y < gameInstance.DimY; y++)
        {
            if (y < 10)
            {
                Console.Write(" " + y + " " + " ");
            }
            else if (y < 100)
            {
                Console.Write(" " + y + " ");
            }
            
            for (var x = 0; x < gameInstance.DimX; x++)
            {
                if (gameInstance.GameGrid.IsUnderGridArea(x, y))
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(" " + EnumHelper.DrawGamePiece(gameInstance.GameBoard[x][y]) + " ");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Gray;

                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(" " + EnumHelper.DrawGamePiece(gameInstance.GameBoard[x][y]) + " ");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Gray;
                    
                }
                if (x != gameInstance.DimX - 1)
                {
                    Console.Write(" ");
                }
            }
            Console.WriteLine();
        }
    }
}