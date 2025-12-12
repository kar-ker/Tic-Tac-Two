using System.Text.Json;
using DAL;
using Domain;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages;

public class GameModel : PageModel
{
    private readonly IGameRepository _gameRepository;

    public GameModel(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }
    
    [BindProperty(SupportsGet = true)]
    public string? Error  { get; set; }
    
    public TicTacTwoBrain GameInstance { get; set; } = default!;

    [BindProperty(SupportsGet = true)] public Guid? Id { get; set; }

    [BindProperty(SupportsGet = true)] public string? Mode { get; set; }

    [BindProperty] public string? PlayerName { get; set; }

    [BindProperty] public string? PlayerPass { get; set; }

    public Player LoggedPlayer { get; set; } = default!;
    
    public bool GameOver { get; set; } = false;

    public IActionResult OnGet()
    {
        if (Id == null)
        {
            return RedirectToPage("/SaveGames/Index", new { error = "no such game, id was null" });
        }

        if (Mode == null)
        {
            return RedirectToPage("/SaveGames/Index", new { error = "Error trying to load game, no mode selected" });
        }
        var mode = EnumHelper.GetEGameMode(Mode);

        if (Request.Query.ContainsKey("playerPass"))
        {
            PlayerPass = Request.Query["playerPass"].ToString();
        }
        if (Request.Query.ContainsKey("playerName"))
        {
            PlayerName = Request.Query["playerName"].ToString();
        }
        var gameState = _gameRepository.GetGameStateById(Id);
        if (gameState.GameOver)
        {
            GameOver = true;
        }
        if (mode == EGameMode.PvP)
        {
            
            if (PlayerName == null) //if the chosen player doesn't exist
            {
                return RedirectToPage("/SaveGames/Index", new { error = "Invalid player choice, can't be null" });
            }

            if (PlayerPass != null)
            {
                if (PlayerName == gameState.Player1.PlName)
                {
                    if (PlayerPass != gameState.Player1.Password)
                    {
                        return RedirectToPage("/SaveGames/Index", new { error = $"Wrong password: {PlayerPass}, for player {PlayerName}" });
                    }
                    LoggedPlayer = gameState.Player1;
                }
                else if (PlayerName == gameState.Player2.PlName)
                {
                    if (PlayerPass != gameState.Player2.Password)
                    {
                        return RedirectToPage("/SaveGames/Index", new { error = $"Wrong password: {PlayerPass}, for player {PlayerName}" });    
                    }
                    LoggedPlayer = gameState.Player2;
                }
            }
            else
            {
                if (PlayerName == gameState.Player1.PlName)
                {
                    if ("" != gameState.Player1.Password)
                    {
                        return RedirectToPage("/SaveGames/Index", new { error = "Password mismatch1" });
                    }

                    LoggedPlayer = gameState.Player1;
                }

                if (PlayerName == gameState.Player2.PlName)
                {
                    if ("" != gameState.Player2.Password)
                    {
                        return RedirectToPage("/SaveGames/Index", new { error = "Password mismatch2" });
                    }

                    LoggedPlayer = gameState.Player2;
                }
            }
            GameInstance = new TicTacTwoBrain(gameState);
            return Page();
        }
        if (mode == EGameMode.PvAi)
        {
            if (PlayerName == null)
            {
                return RedirectToPage("/SaveGames/Index", new { error = "Invalid player choice, can't be null" });
            }

            if (PlayerPass != null)
            {
                if (PlayerName == gameState.Player1.PlName && PlayerPass == gameState.Player1.Password)
                {
                    LoggedPlayer = gameState.Player1;
                }
                else
                {
                    return RedirectToPage("/SaveGames/Index", new { error = $"Wrong password: {PlayerPass}, for player {PlayerName}" });
                }
            }
            else
            {
                if (PlayerName == gameState.Player1.PlName && "" == gameState.Player1.Password)
                {
                    LoggedPlayer = gameState.Player1;
                }
                else
                {
                    return RedirectToPage("/SaveGames/Index", new { error = "Password mismatch1" });
                }
            }
            GameInstance = new TicTacTwoBrain(gameState);
            return Page();
        }
        if (mode == EGameMode.AivAi)
        {
            if (PlayerName == null)
            {
                return RedirectToPage("/SaveGames/Index", new { error = "Invalid player choice, can't be null" });
            }

            if (PlayerPass != null)
            {
                if (PlayerName == "Spectator" && PlayerPass == gameState.Player1.Password)
                {
                    LoggedPlayer = new Player("Spectator", EGamePiece.Empty, 0, PlayerPass);
                }
                else
                {
                    return RedirectToPage("/SaveGames/Index", new { error = $"Wrong password: {PlayerPass}, for player {PlayerName}" });
                }
            }
            else
            {
                if (PlayerName == "Spectator" && "" == gameState.Player1.Password)
                {
                    LoggedPlayer = new Player("Spectator", EGamePiece.Empty, 0, "");
                }
                else
                {
                    return RedirectToPage("/SaveGames/Index", new { error = "Password mismatch1" });
                }
            }
            GameInstance = new TicTacTwoBrain(gameState);
            return Page();
        }
        return NotFound();
    }

    [BindProperty]
    public string Command { get; set; } = default!;
    
    [BindProperty]
    public string GameStateJson { get; set; } = default!;
    [BindProperty]
    public string ConfigName { get; set; } = default!;
    
    [BindProperty]
    public string GridOffset { get; set; } = default!;
    
    [BindProperty]
    public string SecondPieceCoordinates { get; set; } = default!;
    
    [BindProperty]
    public string FirstPieceCoordinates { get; set; } = default!;
    
    public IActionResult OnPost()
    {
        ViewData["Title"] = "Game";
        if (string.IsNullOrEmpty(Command))
        {
            return RedirectToPage("", new
            { id = Id, mode = Mode, playerName = PlayerName, playerPass = PlayerPass,
                error = $"no such command{Command}" });
        }
        
        if (Command == "ExitTheGame")
        {
            return RedirectToPage("SaveGames/Index", new {message = "exit"});
        }
        
        if (Id == null)
        {
            return RedirectToPage("/SaveGames/Index", new { id = Id, mode = Mode, playerName = PlayerName, playerPass = PlayerPass,
                error = $"no such game with Id: {Id}" });
        }
        if (string.IsNullOrEmpty(PlayerPass))
        {
            PlayerPass = "";
        }
        if (Command == "Empty")
        {
            return RedirectToPage("", new
            { id = Id, mode = Mode, playerName = PlayerName, playerPass = PlayerPass,
                error = "Make a move before clicking submit!" });
        }
        var gameState = JsonSerializer.Deserialize<GameState>(GameStateJson);
        if (gameState == null)
        {
            return RedirectToPage("", new {id = Id, mode = Mode, playerName = PlayerName, playerPass = PlayerPass,
                error = "no game state found" });
        }
        
        if (Command == "SaveTheGame")
        {
            _gameRepository.SaveTheGame(GameStateJson, ConfigName);
            return RedirectToPage("", new {id = Id, mode = Mode, playerName = PlayerName, playerPass = PlayerPass});
        }

        if (Command == "SaveAndExitTheGame")
        {
            _gameRepository.SaveTheGame(GameStateJson, ConfigName);
            return RedirectToPage("SaveGames/Index", new {message = "saved"});
        }
        
        var gameInstance = new TicTacTwoBrain(gameState);
        if (Command == "AiMove")
        {
            if (gameInstance.NextMoveBy.PlPiecesOnHand != 0)
            {
                var status = TicAi.PlaceNewPiece(gameInstance);
                if (status != "success")
                {
                    return RedirectToPage("", new {id = Id, mode = Mode, playerName = PlayerName, playerPass = PlayerPass,
                        error = status });
                }
                _gameRepository.UpdateTheGame(Id, gameInstance.GetGameStateJson());
            
                return RedirectToPage("", new { id = Id, mode = Mode, playerName = PlayerName, playerPass = PlayerPass});
            }
            return RedirectToPage("", new {id = Id, mode = Mode, playerName = PlayerName, playerPass = PlayerPass,
                error = "not implemented relocation" });
        }
        
        if (PlayerName != gameInstance.NextMoveBy.PlName || PlayerPass != gameInstance.NextMoveBy.Password)
        {
            return RedirectToPage("", new {id = Id, mode = Mode, playerName = PlayerName, playerPass = PlayerPass,
                error = "It is not your turn to make a move right now!" });
        }
        
        if (Command == "RelocateTheGrid")
        {
            if (gameState.TurnsPlayed < gameState.GameConfiguration.StartMovingGridOnTurnN)
            {
                return RedirectToPage("", new { id = Id, mode = Mode, playerName = PlayerName, playerPass = PlayerPass,
                    error = "Can't move the grid yet." });
            }
            if (string.IsNullOrEmpty(GridOffset))
            {
                return RedirectToPage("", new {id = Id, mode = Mode, playerName = PlayerName, playerPass = PlayerPass,
                    error = "null or empty given for offset" });
            }
            var offset = GridOffset.Split(",");
            if (offset.Length != 2 || !int.TryParse(offset[0], out int x) || !int.TryParse(offset[1], out int y))
            {
                return RedirectToPage("", new {id = Id, mode = Mode, playerName = PlayerName, playerPass = PlayerPass,
                    error = "non integer values given for grid offset" });
            }
            var status = gameInstance.RelocateTheGridWithOffset(x, y);
            if (status != "success") 
            {
                return RedirectToPage("", new {id = Id, mode = Mode, playerName = PlayerName, playerPass = PlayerPass,
                    error = status });
            }
            _gameRepository.UpdateTheGame(Id, gameInstance.GetGameStateJson());
            
            return RedirectToPage("", new { id = Id, mode = Mode, playerName = PlayerName, playerPass = PlayerPass});
        }
        
        if (Command == "PlaceAPiece")
        {
            if (string.IsNullOrEmpty(FirstPieceCoordinates))
            {
                return RedirectToPage("", new { id = Id, mode = Mode, playerName = PlayerName, playerPass = PlayerPass,
                    error = "null or empty given for coordinates" });
            }
            var newPieceCoords = FirstPieceCoordinates.Split(",");
            if (newPieceCoords.Length != 2 || !int.TryParse(newPieceCoords[0], out int x) || !int.TryParse(newPieceCoords[1], out int y))
            {
                return RedirectToPage("", new { id = Id, mode = Mode, playerName = PlayerName, playerPass = PlayerPass,
                    error = "non integer values given for new piece" });
            }
            var status = gameInstance.PlaceAPiece(x, y);
            if (status != "success")
            {
                return RedirectToPage("", new { id = Id, mode = Mode, playerName = PlayerName, playerPass = PlayerPass,
                    error = status });
            }
            _gameRepository.UpdateTheGame(Id, gameInstance.GetGameStateJson());
            
            return RedirectToPage("", new { id = Id, mode = Mode, playerName = PlayerName, playerPass = PlayerPass});
        }
        
        if (Command == "RelocateAPiece")
        {
            if (gameState.TurnsPlayed < gameState.GameConfiguration.StartMovingPieceOnTurnN)
            {
                return RedirectToPage("", new { id = Id, mode = Mode, playerName = PlayerName, playerPass = PlayerPass, 
                    error = "Can't relocate a piece yet." });
            }
            if (string.IsNullOrEmpty(FirstPieceCoordinates) || string.IsNullOrEmpty(SecondPieceCoordinates))
            {
                return RedirectToPage("", new { id = Id, mode = Mode, playerName = PlayerName, playerPass = PlayerPass,
                    error = "null or empty given for coordinates" });
            }
            var oldPieceCoords = FirstPieceCoordinates.Split(",");
            if (oldPieceCoords.Length != 2 || !int.TryParse(oldPieceCoords[0], out int x1) || !int.TryParse(oldPieceCoords[1], out int y1))
            {
                return RedirectToPage("", new { id = Id, mode = Mode, playerName = PlayerName, playerPass = PlayerPass,
                    error = "non integer values given for old piece" });
            }
            var newPieceCoords = SecondPieceCoordinates.Split(",");
            if (newPieceCoords.Length != 2 || !int.TryParse(newPieceCoords[0], out int x2) || !int.TryParse(newPieceCoords[1], out int y2))
            {
                return RedirectToPage("", new {id = Id, mode = Mode, playerName = PlayerName, playerPass = PlayerPass,
                    error = "non integer values given for new piece" });
            }
            var status = gameInstance.RelocateAPiece(x1, y1, x2, y2);
            if (status != "success")
            {
                return RedirectToPage("", new { id = Id, mode = Mode, playerName = PlayerName, playerPass = PlayerPass,
                    error = status });
            }
            _gameRepository.UpdateTheGame(Id, gameInstance.GetGameStateJson());
            
            return RedirectToPage("", new { id = Id, mode = Mode, playerName = PlayerName, playerPass = PlayerPass});
        }
        
        
        return NotFound();
        
        
    }
}
