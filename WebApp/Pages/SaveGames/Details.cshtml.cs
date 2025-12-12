using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.SaveGames
{
    public class DetailsModel : PageModel
    {
        public IGameRepository GameRepo;

        public DetailsModel(IGameRepository gamerepository)
        {
            GameRepo = gamerepository;
        }
        
        [BindProperty]
        public SaveGame SaveGame { get; set; } = default!;
        
        public IActionResult OnGet(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SaveGame = GameRepo.GetSaveGameById(id);
            return Page();
        }
        
        public  IActionResult OnPost(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var status = GameRepo.DeleteSaveGame(id);
            if (status != "success")
            {
                return NotFound();
            }

            return RedirectToPage("./Index");
        }
    }
}
