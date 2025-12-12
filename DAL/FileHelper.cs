using System.Text.Json;
using Domain;
using GameBrain;

namespace DAL;

public static class FileHelper
{
    public static string BasePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
                                        + Path.DirectorySeparatorChar + "tic-tac-two" + Path.DirectorySeparatorChar;
    
    public static string NameAndTimeSeparator = " - ";

    public static string DateTimeNow() => DateTime.Now.ToString("dd.MM.yyyy_HH-mm-ss");

    public const string ConfigExtension = ".config.json";
    public const string GameExtension = ".game.json";

    public static JsonSerializerOptions GetMyJsonOptions()
    {
        return new JsonSerializerOptions()
        {
            IncludeFields = true
        };
    }

    public static void CheckAndCreateDirectory()
    {
        if (!Directory.Exists(BasePath))
        {
            Directory.CreateDirectory(BasePath);
        }
    }

    public static string GetNameAndTimeFromFileName(string name)
    {
        var splitname = name.Split(NameAndTimeSeparator);
        var nameAndTime = splitname[0] +
                          NameAndTimeSeparator +
                          splitname[1];
        return nameAndTime;
    }
    
    public static string GetGuidFromFileName(string name)
    {
        var guid = name.Split(NameAndTimeSeparator)[2];
        return guid;
    }
    
    public static string GetDateFromFileName(string name)
    {
        var date = name.Split(NameAndTimeSeparator)[1];
        return date;
    }
    
    public static string GetNameFromFileName(string name)
    {
        var date = name.Split(NameAndTimeSeparator)[0];
        return date;
    }
    public static string GetCleanFileName(string fileName)
    {
        var name = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(fileName));
        return name;
    }
    
    public static GameConfiguration DomainConfToGameConf(Configuration configuration)
    {
        return new GameConfiguration()
        {
            Name = configuration.ConfigName,
            BoardSizeHeight = configuration.BoardSizeHeight,
            BoardSizeWidth = configuration.BoardSizeWidth,
            WinCondition = configuration.WinCondition,
            GridSide = configuration.GridSide,
            GridX = configuration.GridX,
            GridY = configuration.GridY,
            NumberOfPieces = configuration.NumberOfPieces,
            StartMovingPieceOnTurnN = configuration.StartMovingPieceOnTurnN,
            StartMovingGridOnTurnN = configuration.StartMovingGridOnTurnN
        };
    }
    
    public static Configuration GameConfToDomainConf(GameConfiguration gameConfiguration, Guid guid, string creationTime)
    {
        return new Configuration()
        {
            Id = guid,
            ConfigName = gameConfiguration.Name,
            BoardSizeHeight = gameConfiguration.BoardSizeHeight,
            BoardSizeWidth = gameConfiguration.BoardSizeWidth,
            WinCondition = gameConfiguration.WinCondition,
            GridSide = gameConfiguration.GridSide,
            GridX = gameConfiguration.GridX,
            GridY = gameConfiguration.GridY,
            NumberOfPieces = gameConfiguration.NumberOfPieces,
            StartMovingPieceOnTurnN = gameConfiguration.StartMovingPieceOnTurnN,
            StartMovingGridOnTurnN = gameConfiguration.StartMovingGridOnTurnN,
            CreatedAtDateTime = creationTime,
            SaveGames = null
        };
    }
    
}