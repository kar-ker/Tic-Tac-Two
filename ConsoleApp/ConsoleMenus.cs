using Dal;
using DAL;
using MenuSystem;

namespace ConsoleApp;

public class ConsoleMenus
{
    private static IConfigRepository _configRepository = default!;
    private static IGameRepository _gameRepository = default!;


    public static void InitializeRepos(IConfigRepository configRepository, IGameRepository gameRepository)
    {
        _configRepository = configRepository;
        _gameRepository = gameRepository;
    }

    public static Menu OptionsMenu = new Menu(EMenuLevel.Secondary, "TIC-TAC-TWO Options",
    [
        new MenuItem()
        {
            Shortcut = "C", Title = "Create your own game configuration",
            MenuItemAction = () => CreateConfiguration.MakeNewConfig(_configRepository)
        }
    ]);

    public static Menu MainMenu = new Menu(EMenuLevel.Main, "TIC-TAC-TWO",
    [
        new MenuItem()
        {
            Shortcut = "N",
            Title = "New Game",
            MenuItemAction = () => NewGameController.DisplayMenuAndPlayGame(_configRepository, _gameRepository)
        },

        new MenuItem()
        {
            Shortcut = "L",
            Title = "Load save",
            MenuItemAction = () => LoadSaveController.DisplayMenuAndPlayGame(_gameRepository)
        },

        new MenuItem()
        {
            Shortcut = "O",
            Title = "Options",
            MenuItemAction = OptionsMenu.Run
        }
    ]);

    private static string NotImplementedMethod()
    {
        Console.Write("You have entered into undeveloped territory. Just press any key to get out from here!");
        Console.ReadKey();
        return "BLANK";
    }
}