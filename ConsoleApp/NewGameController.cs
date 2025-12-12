using Dal;
using DAL;
using GameBrain;
using MenuSystem;

namespace ConsoleApp;

public static class NewGameController
{
    private static IConfigRepository _configRepository = default!;
    private static IGameRepository _gameRepository = default!;

    public static string DisplayMenuAndPlayGame(IConfigRepository configRepository, IGameRepository gameRepository)
    {
        _configRepository = configRepository;
        _gameRepository = gameRepository;

        var chosenConfigShortcut = ChooseConfiguration();
        var strShortcut = "";
        if (!int.TryParse(chosenConfigShortcut, out var configNo))
        {
            if (!chosenConfigShortcut.Equals("C"))
            {
                return chosenConfigShortcut;
            }

            strShortcut = "C";
        }

        GameConfiguration chosenConfig;
        if (strShortcut.Equals("C"))
        {
            var chosenConfigName = CreateConfiguration.MakeNewConfig(_configRepository);
            chosenConfig = _configRepository.GetGameConfigurationByName(chosenConfigName);
        }
        else
        {
            chosenConfig = _configRepository.GetGameConfigurationByName(
                _configRepository.GetConfigurationFullNames()[configNo]
            );
        }

        var chosenGameModeShortcut = ChooseGameMode();
        if (!int.TryParse(chosenGameModeShortcut, out var gameModeNo))
        {
            return chosenGameModeShortcut;
        }

        if (gameModeNo > 2)
        {
            return chosenGameModeShortcut;
        }

        EGameMode chosenGameMode = EnumHelper.GetGameModes()[gameModeNo];
        Player player1;
        Player player2;
        if (chosenGameMode == EGameMode.PvAi)
        {
            player1 = new Player("Player", EGamePiece.X, chosenConfig.NumberOfPieces, "");
            player2 = new Player("Ai", EGamePiece.O, chosenConfig.NumberOfPieces, "");
        }
        else if (chosenGameMode == EGameMode.AivAi)
        {
            player1 = new Player("Ai1", EGamePiece.X, chosenConfig.NumberOfPieces, "");
            player2 = new Player("Ai2", EGamePiece.O, chosenConfig.NumberOfPieces, "");
        }
        else
        {
            player1 = new Player("Player1", EGamePiece.X, chosenConfig.NumberOfPieces, "");
            player2 = new Player("Player2", EGamePiece.O, chosenConfig.NumberOfPieces, "");
        }

        var gameInstance = new TicTacTwoBrain(chosenConfig, player1, player2, chosenGameMode);
        var game = GameLoop.PlayGame(gameInstance, _gameRepository);
        Console.WriteLine("game-> " + game);
        return game;
    }

    private static string ChooseConfiguration()
    {
        var configMenuItems = new List<MenuItem>();
        var names = _configRepository.GetConfigurationFullNames();
        var index = 0;
        foreach (var name in names)
        {
            var returnValue = index.ToString();
            configMenuItems.Add(new MenuItem()
            {
                Title = FileHelper.GetNameFromFileName(name),
                Shortcut = (index + 1).ToString(),
                MenuItemAction = () => returnValue
            });
            index++;
        }

        configMenuItems.Add(new MenuItem()
        {
            Title = "Make your own custom configuration",
            Shortcut = "C",
            MenuItemAction = () => "C"
        });

        var configMenu = new Menu(EMenuLevel.Secondary,
            "TIC-TAC-TWO - choose game config",
            configMenuItems,
            isCustomMenu: true
        );

        return configMenu.Run();
    }

    private static string ChooseGameMode()
    {
        var gameModeMenuItems = new List<MenuItem>();
        var modes = EnumHelper.GetGameModes();
        var index = 0;
        foreach (var mode in modes)
        {
            var returnValue = index.ToString();
            gameModeMenuItems.Add(new MenuItem()
            {
                Title = EnumHelper.EGameModeToString(mode),
                Shortcut = (index + 1).ToString(),
                MenuItemAction = () => returnValue
            });
            index++;
        }


        var gameModeMenu = new Menu(EMenuLevel.Secondary,
            "TIC-TAC-TWO - choose game mode",
            gameModeMenuItems,
            isCustomMenu: true
        );

        return gameModeMenu.Run();
    }
}