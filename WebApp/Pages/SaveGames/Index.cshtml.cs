using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;
using GameBrain;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Pages.SaveGames
{
    public class IndexModel : PageModel
    {
        public IGameRepository GameRepo;

        public IndexModel(IGameRepository gamerepository)
        {
            GameRepo = gamerepository;
        }

        [BindProperty(SupportsGet = true)] public string Error { get; set; } = default!;

        [BindProperty(SupportsGet = true)] public string Player1Pass { get; set; } = "";

        [BindProperty(SupportsGet = true)] public string Player2Pass { get; set; } = "";

        [BindProperty] public Guid Id { get; set; }

        [BindProperty] public string Mode { get; set; } = default!;

        private string SentPlayerPass { get; set; } = default!;

        public IList<SaveGame> SaveGames { get; set; } = default!;

        public void OnGet()
        {
            SaveGames = GameRepo.GetSaveGames();
        }

        public IActionResult OnPost(string playerName)
        {
            if (string.IsNullOrEmpty(Player1Pass))
            {
                Player1Pass = "";
            }

            if (string.IsNullOrEmpty(Player2Pass))
            {
                Player2Pass = "";
            }

            if (EnumHelper.GetEGameMode(Mode) == EGameMode.PvP)
            {
                if (playerName == "Player1")
                {
                    SentPlayerPass = Player1Pass;
                }
                else if (playerName == "Player2")
                {
                    SentPlayerPass = Player2Pass;
                }

                return RedirectToPage("/Game", new
                {
                    id = Id, mode = Mode, playerName = playerName, playerPass = SentPlayerPass
                });
            }
            
            if (EnumHelper.GetEGameMode(Mode) == EGameMode.PvAi)
            {
                if (playerName == "Player")
                {
                    SentPlayerPass = Player1Pass;
                }

                return RedirectToPage("/Game", new
                {
                    id = Id, mode = Mode, playerName = playerName, playerPass = SentPlayerPass
                });
            }
            
            if (EnumHelper.GetEGameMode(Mode) == EGameMode.AivAi)
            {
                if (playerName == "Spectator")
                {
                    SentPlayerPass = Player1Pass;
                }

                return RedirectToPage("/Game", new
                {
                    id = Id, mode = Mode, playerName = playerName, playerPass = SentPlayerPass
                });
            }

            return RedirectToPage();
        }
    }
}