using GameBrain;

namespace DAL;

public static class PremadeGameConfigurations
{
    private static List<GameConfiguration> _gameConfigurations = new()
    {
        new GameConfiguration()
        {
            Name = "Normal 5x5"
            
        },
        new GameConfiguration()
        {
            Name = "BIG",
            BoardSizeHeight = 7,
            BoardSizeWidth = 7,
        }
    };


    public static List<GameConfiguration> GetConfigs()
    {
        return _gameConfigurations;
    }

}