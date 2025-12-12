using System.Text.Json;
using DAL;
using GameBrain;
using MenuSystem;

namespace ConsoleApp;

public static class LoadSaveController
{
    private static IGameRepository _gameRepo = default!;

    public static string DisplayMenuAndPlayGame(IGameRepository gameRepo)
    {
        _gameRepo = gameRepo;

        var chosenSaveGameShortcut = SavedGamesMenu();
        if (!int.TryParse(chosenSaveGameShortcut, out var configNo))
        {
            return chosenSaveGameShortcut;
        }

        var games = _gameRepo.GetSaveGames();
        if (configNo > games.Count - 1 || configNo < 0)
        {
            return "error";
        }
        var chosenSaveGame = JsonSerializer.Deserialize<GameState>(games[configNo].State);
        if (chosenSaveGame == null)
        {
            return "error";
        }
        var gameInstance = new TicTacTwoBrain(chosenSaveGame);
        var game = GameLoop.PlayGame(gameInstance, _gameRepo);

        return game;
    }


    private static string SavedGamesMenu()
    {
        var saveGames = _gameRepo.GetSaveGames();
        var menuItems = new List<MenuItem>();
        var index = 1;
        for (var i = saveGames.Count - 1; i > -1; i--) // reverse the order
        {
            var state = JsonSerializer.Deserialize<GameState>(saveGames[i].State);
            if (state == null)
            {
                throw new NullReferenceException();
            }

            var mode = EnumHelper.EGameModeToString(state.GameMode);
            var displayName = saveGames[i].SaveGameName + " (" + mode + ")";
            if (state.GameOver)
            {
                displayName += " (Game over!)";
            }

            var returnvalue = i.ToString(); // this gives the opposite value underneath, but the user sees it as 1,2,3...
            menuItems.Add(new MenuItem()
            {
                Title = displayName,
                Shortcut = index.ToString(),
                MenuItemAction = () => returnvalue
            });
            index++;
        }

        Menu saveGamesMenu = new Menu(EMenuLevel.Secondary,
            "TIC-TAC-TWO - Load game from save",
            menuItems,
            true
        );
        return saveGamesMenu.Run();
    }
    
}