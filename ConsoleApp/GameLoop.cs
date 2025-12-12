using ConsoleUI;
using DAL;
using GameBrain;

namespace ConsoleApp;

public class GameLoop
{
    public static string PlayGame(TicTacTwoBrain gameInstance, IGameRepository gameRepo)
    {
        do
        {
            if (gameInstance.GameOver)
            {
                do
                {
                    Console.WriteLine($"Game over! Winner is {gameInstance.NextMoveBy.PlName} with symbol {gameInstance.NextMoveBy.PlSymbol}." );
                    Visualizer.DrawBoard(gameInstance);
                    Console.WriteLine("Type exit to exit the game.");
                    var exitInput = Console.ReadLine();
                    if (exitInput == null)
                    {
                        continue;
                    }

                    if (exitInput.Equals("exit", StringComparison.InvariantCultureIgnoreCase) ||
                        exitInput.Equals("e", StringComparison.InvariantCultureIgnoreCase))
                    {
                        Console.WriteLine("exit");
                        return "exit";
                    }
                } while (true);
            }
            Visualizer.DrawBoard(gameInstance);
            bool noAi = gameInstance.NextMoveBy.PlName != "Ai" && gameInstance.NextMoveBy.PlName != "Ai1" &&
                        gameInstance.NextMoveBy.PlName != "Ai2";
            string moveMessage = $"This is turn {gameInstance.TurnsPlayed}.\n";
            moveMessage +=
                $"Current move by {gameInstance.NextMoveBy.PlName} with {gameInstance.NextMoveBy.PlPiecesOnHand} pieces on hand.\n";
            moveMessage += $"The symbol is {gameInstance.NextMoveBy.PlSymbol}.\n";
            if (noAi)
            {
                if (gameInstance.NextMoveBy.PlPiecesOnHand > 0)
                {
                    moveMessage += "To place a new piece, type: P,<x>,<y>\n";
                }

                if (gameInstance.TurnsPlayed >= gameInstance.GameConfiguration.StartMovingGridOnTurnN)
                {
                    moveMessage += "To move the grid, type: G,<x>,<y>\n";
                }

                if (gameInstance.TurnsPlayed >= gameInstance.GameConfiguration.StartMovingPieceOnTurnN)
                {
                    moveMessage += "To relocate a piece, type: R,<x1>,<y1>,<x2>,<y2>\n";
                }
            }
            else
            {
                moveMessage += "to let the Ai make a move, type: ai\n";
            }


            moveMessage += "Type exit to exit to Main Menu or save to save the game.\n";

            Console.Write(moveMessage);

            var input = Console.ReadLine()!;
            if (input.Equals("save", StringComparison.InvariantCultureIgnoreCase) ||
                input.Equals("s", StringComparison.InvariantCultureIgnoreCase))
            {
                gameRepo.SaveTheGame(
                    gameInstance.GetGameStateJson(),
                    gameInstance.GetGameConfigName()
                );
            }
            else if (input.Equals("exit", StringComparison.InvariantCultureIgnoreCase) ||
                     input.Equals("e", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("exit");
                return "exit";
            }
            else
            {
                if (noAi)
                {
                    var inputSplit = input.Split(",");
                    if (inputSplit.Length < 3)
                    {
                        Console.WriteLine("Invalid input");
                        continue;
                    }

                    var inputX1 = int.Parse(inputSplit[1]);
                    var inputY1 = int.Parse(inputSplit[2]);
                    if (inputSplit[0].ToUpper() == "P")
                    {
                        if (gameInstance.NextMoveBy.PlPiecesOnHand <= 0)
                        {
                            Console.WriteLine("Invalid input, you have no more pieces left in hand.");
                            continue;
                        }

                        gameInstance.PlaceAPiece(inputX1, inputY1);
                    }

                    if (inputSplit[0].ToUpper() == "G" &&
                        gameInstance.TurnsPlayed >= gameInstance.GameConfiguration.StartMovingGridOnTurnN)
                    {
                        gameInstance.RelocateTheGrid(inputX1, inputY1);
                    }

                    if (inputSplit.Length == 5 && inputSplit[0].ToUpper() == "R" &&
                        gameInstance.TurnsPlayed >= gameInstance.GameConfiguration.StartMovingPieceOnTurnN)
                    {
                        var inputX2 = int.Parse(inputSplit[3]);
                        var inputY2 = int.Parse(inputSplit[4]);
                        gameInstance.RelocateAPiece(inputX1, inputY1, inputX2, inputY2);
                    }
                }
                else if (input.Equals("ai", StringComparison.InvariantCultureIgnoreCase) ||
                         input.Equals("a", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (gameInstance.NextMoveBy.PlPiecesOnHand <= 0)
                    {
                        Console.WriteLine("Ai doesn't know how to relocate pieces :(");
                        continue;
                    }
                    TicAi.PlaceNewPiece(gameInstance);
                }
            }
        } while (true);
    }
}