using DAL;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Model;
using Microsoft.IdentityModel.Tokens;

namespace WebApp.Pages;

public class GameModeOptionsModel : PageModel
{
    private readonly IConfigRepository _configRepository;
    private readonly IGameRepository _gameRepository;

    public GameModeOptionsModel(IConfigRepository configRepository, IGameRepository gameRepository)
    {
        _configRepository = configRepository;
        _gameRepository = gameRepository;
    }


    [BindProperty(SupportsGet = true)] public Guid Id { get; set; }

    [BindProperty(SupportsGet = true)] public string Mode { get; set; } = default!;

    public List<SelectListItem> SymbolOptions { get; set; } = default!;
    public List<SelectListItem> PlayerOptions { get; set; } = default!;

    [BindProperty] public string SelectedSymbol { get; set; } = default!;
    [BindProperty] public string StartingName { get; set; } = default!;

    [BindProperty] public string Player1Pass { get; set; } = default!;

    [BindProperty] public string Player2Pass { get; set; } = default!;


    private string SentPlayerPass { get; set; } = default!;


    public IActionResult OnGet()
    {
        if (Mode.IsNullOrEmpty())
        {
            return NotFound();
        }

        var mode = EnumHelper.GetEGameMode(Mode);
        SelectedSymbol = EnumHelper.EGamePieceToString(EGamePiece.X);
        SymbolOptions = new List<SelectListItem>
        {
            new SelectListItem { Text = "X", Value = EnumHelper.EGamePieceToString(EGamePiece.X) },
            new SelectListItem { Text = "O", Value = EnumHelper.EGamePieceToString(EGamePiece.O) }
        };
        if (mode == EGameMode.PvP)
        {
            StartingName = "Player1";
            PlayerOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Player1", Value = "Player1" },
                new SelectListItem { Text = "Player2", Value = "Player2" }
            };
            return Page();
        }

        if (mode == EGameMode.PvAi)
        {
            StartingName = "Player";
            PlayerOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Player", Value = "Player" },
                new SelectListItem { Text = "Ai", Value = "Ai" }
            };
            return Page();
        }

        if (mode == EGameMode.AivAi)
        {
            return Page();
        }

        return NotFound();
    }

    public IActionResult OnPost()
    {
        var gameConfiguration = _configRepository.GetGameConfigurationById(Id);
        var symbol1 = EnumHelper.GetEGamePiece(SelectedSymbol);
        var symbol2 = symbol1 == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
        var mode = EnumHelper.GetEGameMode(Mode);

        if (mode == EGameMode.PvP)
        {
            Player player1 = new Player("Player1", symbol1, gameConfiguration.NumberOfPieces, Player1Pass ?? "");
            Player player2 = new Player("Player2", symbol2, gameConfiguration.NumberOfPieces, Player2Pass ?? "");
            var gameInstance = new TicTacTwoBrain(gameConfiguration, player1, player2, mode);
            if (StartingName == "Player1")
            {
                SentPlayerPass = Player1Pass;
                gameInstance.NextMoveBy = player1;
            }
            else if (StartingName == "Player2")
            {
                SentPlayerPass = Player2Pass;
                gameInstance.NextMoveBy = player2;
            }

            var guid = _gameRepository.SaveTheGame(gameInstance.GetGameStateJson(), gameConfiguration.Name);

            return RedirectToPage("/Game", new
            {
                id = guid, mode = Mode, playerName = StartingName, playerPass = SentPlayerPass
            });
        }

        if (mode == EGameMode.PvAi)
        {
            Player player1 = new Player("Player", symbol1, gameConfiguration.NumberOfPieces, Player1Pass ?? "");
            Player player2 = new Player("Ai", symbol2, gameConfiguration.NumberOfPieces, "");
            var gameInstance = new TicTacTwoBrain(gameConfiguration, player1, player2, mode);
            if (StartingName == "Player")
            {
                gameInstance.NextMoveBy = player1;
            }
            else
            {
                gameInstance.NextMoveBy = player2;
            }

            var guid = _gameRepository.SaveTheGame(gameInstance.GetGameStateJson(), gameConfiguration.Name);

            return RedirectToPage("/Game", new
            {
                id = guid, mode = Mode, playerName = "Player", playerPass = Player1Pass
            });
        }
        
        if (mode == EGameMode.AivAi)
        {
            Player player1 = new Player("Ai1", symbol1, gameConfiguration.NumberOfPieces, Player1Pass ?? "");
            Player player2 = new Player("Ai2", symbol2, gameConfiguration.NumberOfPieces, "");
            var gameInstance = new TicTacTwoBrain(gameConfiguration, player1, player2, mode);

            var guid = _gameRepository.SaveTheGame(gameInstance.GetGameStateJson(), gameConfiguration.Name);

            return RedirectToPage("/Game", new
            {
                id = guid, mode = Mode, playerName = "Spectator", playerPass = Player1Pass
            });
        }
        return RedirectToPage("/Configurations/Index", new {error = "No mode chosen" });
    }
}