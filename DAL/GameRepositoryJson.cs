using System.Text.Json;
using Domain;
using GameBrain;

namespace DAL;

public class GameRepositoryJson : IGameRepository
{
    public Guid SaveTheGame(string state, string gameConfigName)
    {
        var guid = Guid.NewGuid();
        FileHelper.CheckAndCreateDirectory();
        var fileName = FileHelper.BasePath +
                       gameConfigName +
                       FileHelper.NameAndTimeSeparator +
                       FileHelper.DateTimeNow() +
                       FileHelper.NameAndTimeSeparator +
                       guid +
                       FileHelper.GameExtension;
        File.WriteAllText(fileName, state);
        return guid;
    }

    public Guid UpdateTheGame(Guid? id, string stateJson)
    {
        if (id == null)
        {
            throw new NullReferenceException($"id is null");
        }
        FileHelper.CheckAndCreateDirectory();
        var files = Directory.GetFiles(FileHelper.BasePath, "*" + FileHelper.GameExtension);
        foreach (var file in files)
        {
            var cleanFileName = FileHelper.GetCleanFileName(file);
            var guid = Guid.Parse(FileHelper.GetGuidFromFileName(cleanFileName));
            if (guid != id) continue;
            
            File.WriteAllText(file, stateJson);
            
            return (Guid) id;
        }
        throw new FileNotFoundException($"Game not found by id: {id}");
    }

    public List<SaveGame> GetSaveGames()
    {
        FileHelper.CheckAndCreateDirectory();
        var gameStates = new List<SaveGame>();
        var files = Directory.GetFiles(FileHelper.BasePath, "*" + FileHelper.GameExtension);
        foreach (var file in files)
        {
            var gameStateJson = File.ReadAllText(file);
            var gameState = JsonSerializer.Deserialize<GameState>(gameStateJson, FileHelper.GetMyJsonOptions());
            if (gameState == null) continue;
            var gameConfig = gameState.GameConfiguration;
            var cleanFileName = FileHelper.GetCleanFileName(file);
            var saveGame = new SaveGame()
            {
                Id = Guid.Parse(FileHelper.GetGuidFromFileName(cleanFileName)),
                SaveGameName = FileHelper.GetNameAndTimeFromFileName(cleanFileName),
                CreatedAtDateTime = FileHelper.GetDateFromFileName(cleanFileName),
                State = gameStateJson,
                Configuration = new Configuration()
                {
                    ConfigName = gameConfig.Name,
                    BoardSizeHeight = gameConfig.BoardSizeHeight,
                    BoardSizeWidth = gameConfig.BoardSizeWidth,
                    WinCondition = gameConfig.WinCondition,
                    GridSide = gameConfig.GridSide,
                    GridX = gameConfig.GridX,
                    GridY = gameConfig.GridY,
                    NumberOfPieces = gameConfig.NumberOfPieces,
                    StartMovingPieceOnTurnN = gameConfig.StartMovingPieceOnTurnN,
                    StartMovingGridOnTurnN = gameConfig.StartMovingGridOnTurnN,
                    CreatedAtDateTime = FileHelper.DateTimeNow(),
                    SaveGames = null
                }
            };
            gameStates.Add(saveGame);
        }
        return gameStates.OrderBy(s => s.CreatedAtDateTime).ToList();
    }

    public GameState GetGameStateById(Guid? id)
    {
        FileHelper.CheckAndCreateDirectory();
        var files = Directory.GetFiles(FileHelper.BasePath, "*" + FileHelper.GameExtension);
        foreach (var file in files)
        {
            var cleanFileName = FileHelper.GetCleanFileName(file);
            var guid = Guid.Parse(FileHelper.GetGuidFromFileName(cleanFileName));
            if (guid != id) continue;
            
            var gameStateJson = File.ReadAllText(file);
            var gameState = JsonSerializer.Deserialize<GameState>(gameStateJson, FileHelper.GetMyJsonOptions());
            if (gameState != null)
            {
                return gameState;
            }
            throw new FileNotFoundException($"Deserialized GameState is null. From SaveGame id: {id}");
        }
        throw new FileNotFoundException($"Game not found by id: {id}");
    }
    
    public SaveGame GetSaveGameById(Guid? id)
    {
        var saveGames = GetSaveGames();
        foreach (var saveGame in saveGames)
        {
            if (saveGame.Id == id)
            {
                return saveGame;
            }
        }
        throw new FileNotFoundException($"Game not found by id: {id}");
    }

    public string DeleteSaveGame(Guid? id)
    {
        var files = Directory.GetFiles(FileHelper.BasePath, "*" + FileHelper.GameExtension);
        foreach (var file in files)
        {
            var cleanFileName = FileHelper.GetCleanFileName(file);
            var guid = Guid.Parse(FileHelper.GetGuidFromFileName(cleanFileName));
            if (guid != id) continue;
            File.Delete(file);
            return "success";
        }
        return "Failed. No such SaveGame found.";
    }
}