using System.Text.Json;
using Domain;
using GameBrain;

namespace DAL;

public class GameRepositoryDb : IGameRepository
{
    private readonly AppDbContext _context;

    public GameRepositoryDb(AppDbContext context)
    {
        _context = context;
    }
    
    public Guid SaveTheGame(string state, string gameConfigName)
    {
        Configuration config;
        try
        {
            config = GetConfigurationByName(gameConfigName);
        }
        catch (SystemException e)
        {
            throw new Exception(e.Message);
        }

        var creationTime = FileHelper.DateTimeNow();
        var saveGame = new SaveGame()
        {
            SaveGameName = gameConfigName + FileHelper.NameAndTimeSeparator + creationTime,
            CreatedAtDateTime = creationTime,
            State = state,
            Configuration = config
        };

        _context.SaveGames.Add(saveGame);
        _context.SaveChanges();
        return saveGame.Id;
    }

    public Guid UpdateTheGame(Guid? gameId, string stateJson)
    {
        if (gameId == null)
        {
            throw new ArgumentNullException(nameof(gameId));
        }
        var game = _context.SaveGames.FirstOrDefault(s => s.Id == gameId);
        if (game == null)
        {
            throw new KeyNotFoundException();
        }
        game.State = stateJson;
        _context.SaveChanges();
        return (Guid) gameId; // can never be null
    }

    public List<SaveGame> GetSaveGames()
    {
        return _context.SaveGames.ToList();
    }

    public GameState GetGameStateById(Guid? id)
    {
        var saveGame = _context.SaveGames.FirstOrDefault(s => s.Id == id);
        if (saveGame == null)
        {
            throw new SystemException($"Game not found by id: {id}");
        }

        var gameStateJson = saveGame.State;
        var gameState = JsonSerializer.Deserialize<GameState>(gameStateJson, FileHelper.GetMyJsonOptions());
        if (gameState != null)
        {
            return gameState;
        }
        throw new FileNotFoundException($"Deserialized GameState is null. From SaveGame id: {id}");
    }
    
    public SaveGame GetSaveGameById(Guid? id)
    {
        var saveGame = _context.SaveGames.FirstOrDefault(s => s.Id == id);
        if (saveGame == null)
        {
            throw new SystemException($"Game not found by id: {id}");
        }

        return saveGame;
    }

    public string DeleteSaveGame(Guid? id)
    {
        try
        {
            _context.SaveGames.Remove(GetSaveGameById(id));
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            return e.Message;
        }
        return "success";
    }

    private Configuration GetConfigurationByName(string? gameConfigName)
    {
        var dbConfig = _context.Configurations.FirstOrDefault(c => c.ConfigName == gameConfigName);
        if (dbConfig == null)
        {
            throw new SystemException($"Game configuration not found by name: {gameConfigName}");
        }
        
        return dbConfig;
    }
}