using Domain;
using GameBrain;

namespace DAL;

public interface IConfigRepository
{
    public void SaveConfig(GameConfiguration gameConfiguration);

    public void SaveConfig(Configuration configuration);
    
    List<Configuration> GetConfigurations();
    
    List<string> GetConfigurationFullNames();

    GameConfiguration GetGameConfigurationByName(string name);
    
    GameConfiguration GetGameConfigurationById(Guid? id);
    
    Configuration GetConfigurationById(Guid? id);

    string DeleteConfiguration(Guid? id);
}