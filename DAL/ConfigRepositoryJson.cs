using System.Text.Json;
using Domain;
using GameBrain;

namespace DAL;

public class ConfigRepositoryJson : IConfigRepository
{
    public void SaveConfig(GameConfiguration gameConfiguration)
    {
        FileHelper.CheckAndCreateDirectory();
        var fileName = FileHelper.BasePath +
                       gameConfiguration.Name +
                       FileHelper.NameAndTimeSeparator +
                       FileHelper.DateTimeNow() +
                       FileHelper.NameAndTimeSeparator +
                       Guid.NewGuid() +
                       FileHelper.ConfigExtension;
        File.WriteAllText(fileName, JsonSerializer.Serialize(gameConfiguration));
    }
    
    public void SaveConfig(Configuration configuration)
    {
        FileHelper.CheckAndCreateDirectory();
        var fileName = FileHelper.BasePath +
                       configuration.ConfigName +
                       FileHelper.NameAndTimeSeparator +
                       FileHelper.DateTimeNow() +
                       FileHelper.NameAndTimeSeparator +
                       Guid.NewGuid() +
                       FileHelper.ConfigExtension;
        File.WriteAllText(fileName, JsonSerializer.Serialize(FileHelper.DomainConfToGameConf(configuration)));
    }

    public List<Configuration> GetConfigurations()
    {
        List<Configuration> configurations = new List<Configuration>();
        var files = Directory.GetFiles(FileHelper.BasePath, "*" + FileHelper.ConfigExtension);
        foreach (var file in files)
        {
            var fileJson = File.ReadAllText(file);
            var cleanFileName = FileHelper.GetCleanFileName(file);
            var gameConfiguration = JsonSerializer.Deserialize<GameConfiguration>(fileJson, FileHelper.GetMyJsonOptions());
            var configuration = FileHelper.GameConfToDomainConf(gameConfiguration,
                Guid.Parse(FileHelper.GetGuidFromFileName(cleanFileName)),
                FileHelper.GetDateFromFileName(cleanFileName));
            configurations.Add(configuration);
        }
        return configurations;
    }

    public List<string> GetConfigurationFullNames()
    {
        CheckAndCreateInitialConfigs();
        return Directory.GetFiles(FileHelper.BasePath, "*" + FileHelper.ConfigExtension)
            .Select(FileHelper.GetCleanFileName)
            .ToList();
    }

    public GameConfiguration GetGameConfigurationByName(string name)
    {
        CheckAndCreateInitialConfigs();
        var configJsonStr = File.ReadAllText(FileHelper.BasePath + name + FileHelper.ConfigExtension);
        var config = JsonSerializer.Deserialize<GameConfiguration>(configJsonStr);
        return config;
    }

    public GameConfiguration GetGameConfigurationById(Guid? id)
    {
        CheckAndCreateInitialConfigs();
        var files = Directory.GetFiles(FileHelper.BasePath, "*" + FileHelper.ConfigExtension);
        foreach (var file in files)
        {
            var cleanFileName = FileHelper.GetCleanFileName(file);
            var guid = Guid.Parse(FileHelper.GetGuidFromFileName(cleanFileName));
            if (guid != id) continue;
            
            var gameConfJson = File.ReadAllText(file);
            var gameConf = JsonSerializer.Deserialize<GameConfiguration>(gameConfJson, FileHelper.GetMyJsonOptions());
            if (gameConf != null)
            {
                return gameConf;
            }
            throw new FileNotFoundException($"Deserialized GameConfiguration is null. From Configuration id: {id}");
        }
        throw new FileNotFoundException($"GameConfiguration not found by id: {id}");
    }

    public Configuration GetConfigurationById(Guid? id)
    {
        var configurations = GetConfigurations();
        foreach (var conf in configurations)
        {
            if (conf.Id == id)
            {
                return conf;
            }
        }
        throw new FileNotFoundException($"Configuration not found by id: {id}");
    }

    public string DeleteConfiguration(Guid? id)
    {
        var files = Directory.GetFiles(FileHelper.BasePath, "*" + FileHelper.ConfigExtension);
        foreach (var file in files)
        {
            var cleanFileName = FileHelper.GetCleanFileName(file);
            var guid = Guid.Parse(FileHelper.GetGuidFromFileName(cleanFileName));
            if (guid != id) continue;
            File.Delete(file);
            return "success";
        }
        return "Failed. No such Configuration found.";
    }

    private void CheckAndCreateInitialConfigs()
    {
        FileHelper.CheckAndCreateDirectory();
        var data = Directory.GetFiles(FileHelper.BasePath, "*" + FileHelper.ConfigExtension).ToList();
        if (data.Count != 0) return;
        
        var gameConfigs = PremadeGameConfigurations.GetConfigs();
        foreach (var gameConfig in gameConfigs)
        {
            SaveConfig(gameConfig);
        }
    }
}