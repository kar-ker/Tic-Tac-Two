using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;
using GameBrain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages.Configurations
{
    public class IndexModel : PageModel
    {
        private readonly IConfigRepository _configRepository;

        public IndexModel(IConfigRepository configRepository)
        {
            _configRepository = configRepository;
        }

        public IList<Configuration> Configurations { get;set; } = default!;
        
        public List<SelectListItem> Options { get; set; } = default!;
        
        [BindProperty]
        public string SelectedOption { get; set; } = default!;
        
        public void OnGet()
        {
            Configurations = _configRepository.GetConfigurations();
            SelectedOption = EGameMode.PvP.ToString();
            Options = new List<SelectListItem>
            {
                new SelectListItem { Text = "Player vs Player", Value = EGameMode.PvP.ToString() },
                new SelectListItem { Text = "Player vs AI", Value = EGameMode.PvAi.ToString() },
                new SelectListItem { Text = "AI vs AI", Value = EGameMode.AivAi.ToString() }
            };
        }
        

        public IActionResult OnPost(Guid id)
        {
            return RedirectToPage("/GameModeOptions",new {id=id, mode=SelectedOption});
        }
    }
}
