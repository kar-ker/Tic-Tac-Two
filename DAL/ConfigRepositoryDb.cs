using Domain;
using GameBrain;

namespace DAL;

public class ConfigRepositoryDb : IConfigRepository
{
    private readonly AppDbContext _context;

    public ConfigRepositoryDb(AppDbContext context)
    {
        _context = context;
    }

    public void SaveConfig(GameConfiguration gameConfiguration)
    {
        var configuration = FileHelper.GameConfToDomainConf(gameConfiguration, Guid.NewGuid(), FileHelper.DateTimeNow());
        _context.Configurations.Add(configuration);
        _context.SaveChanges();
    }
    
    public void SaveConfig(Configuration configuration)
    {
        configuration.CreatedAtDateTime = FileHelper.DateTimeNow();
        configuration.Id = Guid.NewGuid();
        _context.Configurations.Add(configuration);
        _context.SaveChanges();
    }
    
    public List<Configuration> GetConfigurations()
    {
        SaveHardcodedConfigs();
        return _context.Configurations.ToList();
    }

    public List<string> GetConfigurationFullNames()
    {
        SaveHardcodedConfigs();
        return _context.Configurations.Select(c => c.ConfigName).ToList();

    }

    public GameConfiguration GetGameConfigurationByName(string name)
    {
        SaveHardcodedConfigs();
        Configuration? conf = _context.Configurations.FirstOrDefault(x => x.ConfigName == name);
        if (conf == null)
        {
            throw new KeyNotFoundException($"Configuration with name {name} not found.");
        }

        return FileHelper.DomainConfToGameConf(conf);
    }
    
    

    public GameConfiguration GetGameConfigurationById(Guid? id)
    {
        SaveHardcodedConfigs();
        Configuration? conf = _context.Configurations.FirstOrDefault(c => c.Id == id);
        if (conf == null)
        {
            throw new SystemException($"Configuration with id {id} not found.");
        }

        return FileHelper.DomainConfToGameConf(conf);
    }

    public Configuration GetConfigurationById(Guid? id)
    {
        SaveHardcodedConfigs();
        var conf = _context.Configurations.FirstOrDefault(s => s.Id == id);
        if (conf == null)
        {
            throw new SystemException($"Configuration not found by id: {id}");
        }

        return conf;
    }

    public string DeleteConfiguration(Guid? id)
    {
            try
            {
                _context.Configurations.Remove(GetConfigurationById(id));
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return "success";
    }

    // This method is made in case we want premade configs to show up at the start.
    private void SaveHardcodedConfigs() 
    {
        if (!_context.Configurations.Any())
        {
            var gameConfigs = PremadeGameConfigurations.GetConfigs();
            foreach (var gameConfig in gameConfigs)
            {
                SaveConfig(gameConfig);
            }
        }
    }
}