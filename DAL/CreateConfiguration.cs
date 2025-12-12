using DAL;
using GameBrain;

namespace Dal;

public static class CreateConfiguration
{
    private static IConfigRepository _configRepository = default!;
    public static string MakeNewConfig(IConfigRepository configRepository)
    {
        _configRepository = configRepository;
        var gameConfig = new GameConfiguration();
        Console.WriteLine("Hello, You made it to the 'Create your own game' menu, Good Job!");
        var message = "For starters, how about You give the new configuration a name? Type it in.";
        do
        {
            gameConfig.Name = AskStrInput(message);
            var existingConfigNames = _configRepository.GetConfigurationFullNames();
            if (existingConfigNames.Exists(name => name.Split(FileHelper.NameAndTimeSeparator)[0]
                    .Equals(gameConfig.Name)))
            {
                message = "Configuration with the name '" + gameConfig.Name + "' already exists. Try again.";
                continue;
            }

            break;
        } while (true);

        message = "Next give me the width of the board.";
        do
        {
            gameConfig.BoardSizeWidth = AskIntInput(message);
            if (gameConfig.BoardSizeWidth < 3)
            {
                message = "Given value is too small to be used as the boards width. Try again.";
                continue;
            }

            break;
        } while (true);


        message = "Now give me the height of the board.";
        do
        {
            gameConfig.BoardSizeHeight = AskIntInput(message);
            if (gameConfig.BoardSizeHeight < 3)
            {
                message = "Given value is too small to be used as the boards length. Try again.";
                continue;
            }

            break;
        } while (true);


        message = "Give the grid its side length.";
        do
        {
            gameConfig.GridSide = AskIntInput(message);
            if (gameConfig.GridSide < 2)
            {
                message = "Given value is too small to be used as the grid's side length. Try again.";
                continue;
            }

            if (gameConfig.GridSide <= gameConfig.BoardSizeWidth && gameConfig.GridSide <= gameConfig.BoardSizeHeight)
            {
                break;
            }

            message = "Invalid value. Grid can't be larger than the board. Try again.";
        } while (true);

        message = "Give the grid's centre point X coordinate.";
        do
        {
            gameConfig.GridX = AskIntInput(message);
            var diff = gameConfig.GridSide / 2;
            var even = (gameConfig.GridSide % 2) == 0; // to check if the grid side is even
            if (gameConfig.GridX - diff >= 0 &&
                (even && gameConfig.GridX + diff <= gameConfig.BoardSizeWidth ||
                !even && gameConfig.GridX + diff <= gameConfig.BoardSizeWidth - 1))
            {
                break;
            }

            message = $"Invalid value. Grid's centre point X coordinate can't be {gameConfig.GridX}. Try again.";
        } while (true);

        message = "Give the grid's centre point Y coordinate.";
        do
        {
            gameConfig.GridY = AskIntInput(message);
            var diff = gameConfig.GridSide / 2;
            var even = (gameConfig.GridSide % 2) == 0; // to check if the grid side is even
            if (gameConfig.GridY - diff >= 0 && 
                (even && gameConfig.GridY + diff <= gameConfig.BoardSizeHeight ||
                !even && gameConfig.GridY + diff <= gameConfig.BoardSizeHeight - 1))
            {
                break;
            }

            message = $"Invalid value. Grid's centre point Y coordinate can't be {gameConfig.GridY}. Try again.";
        } while (true);

        message = "Type in how many pieces a player should have.";
        do
        {
            gameConfig.NumberOfPieces = AskIntInput(message);
            if (gameConfig.NumberOfPieces > 1)
            {
                break;
            }

            message = "Invalid value. The number of pieces you tried to give is way too small. Try again.";
        } while (true);

        message = "Give the amount of pieces a player must get in a row inside the grid to win.";
        do
        {
            gameConfig.WinCondition = AskIntInput(message);
            if (gameConfig.WinCondition > gameConfig.NumberOfPieces)
            {
                message =
                    $"Invalid value. A player has only {gameConfig.NumberOfPieces} pieces, your given value of {gameConfig.WinCondition} for the win condition is too big. Try again.";
                continue;
            }

            if (gameConfig.WinCondition > gameConfig.GridSide)
            {
                message =
                    $"Invalid value. The grid has a side length of {gameConfig.GridSide}, your given value of {gameConfig.WinCondition} for the win condition is too big. Try again.";
                continue;
            }

            break;
        } while (true);


        message = "On what turn can a player pick up and move their piece somewhere else?";
        gameConfig.StartMovingPieceOnTurnN = AskIntInput(message);
        message = "On what turn can a player pick up the grid and place it somewhere else?";
        gameConfig.StartMovingGridOnTurnN = AskIntInput(message);

        Console.WriteLine(gameConfig.ToString());
        _configRepository.SaveConfig(gameConfig);
        return gameConfig.Name;
    }

    private static string AskStrInput(string message)
    {
        Console.WriteLine(message);
        string? input = null;
        while (input == null)
        {
            input = Console.ReadLine();
            if (input == null)
            {
                Console.WriteLine("Try again, you'll get it eventually... right?");
            }
        }

        return input;
    }

    private static int AskIntInput(string message)
    {
        do
        {
            var input = AskStrInput(message);
            if (int.TryParse(input, out var result))
            {
                if (result >= 0)
                {
                    return result;
                }
            }

            Console.WriteLine("You have to give a positive integer.");
        } while (true);
    }
}