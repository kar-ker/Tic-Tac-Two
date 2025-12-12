using Domain;
using GameBrain;

namespace DAL;

public interface IGameRepository
{
    Guid SaveTheGame(string state, string gameConfigName);
    
    Guid UpdateTheGame(Guid? id, string stateJson);
    
    List<SaveGame> GetSaveGames();
    
    GameState GetGameStateById(Guid? id);

    SaveGame GetSaveGameById(Guid? id);
    
    string DeleteSaveGame(Guid? id);


}