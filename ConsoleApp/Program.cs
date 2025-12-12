using ConsoleApp;
using DAL;

using var db = DbHelper.CreateCtx(); // automatically disposes

// var configRepository = new ConfigRepositoryJson();
// var gameRepository = new GameRepositoryJson();
var configRepository = new ConfigRepositoryDb(db);
var gameRepository = new GameRepositoryDb(db);

ConsoleMenus.InitializeRepos(configRepository, gameRepository);
ConsoleMenus.MainMenu.Run();
