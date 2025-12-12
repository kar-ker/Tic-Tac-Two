using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using Domain;
using GameBrain;
using Microsoft.IdentityModel.Tokens;

namespace WebApp.Pages_Configurations
{
    public class CreateModel : PageModel
    {
        private readonly IConfigRepository _configRepository;

        public CreateModel(IConfigRepository configRepository)
        {
            _configRepository = configRepository;
        }
        
        public List<string>? Errors { get; set; }

        public IActionResult OnGet(List<string>? errors)
        {
            if (errors != null)
            {
                Errors = errors;
            }
            return Page();
        }

        [BindProperty] public Configuration Configuration { get; set; } = default!;

        [BindProperty] public string ErrorMessage { get; set; } = default!;

        public Configuration Config { get; set; } = default!;

        public IActionResult OnPost()
        {
            var errors = new List<string>();
            var existingConfigNames = _configRepository.GetConfigurationFullNames();
            if (existingConfigNames.Exists(name => name.Split(FileHelper.NameAndTimeSeparator)[0]
                    .Equals(Configuration.ConfigName)))
            {
                errors.Add("Configuration with the name '" + Configuration.ConfigName + "' already exists. Try again.");
            }
            
            if (Configuration.WinCondition > Configuration.NumberOfPieces)
            {
                errors.Add("WinCondition can't be larger than the number of pieces a player has.");
            }
            if (Configuration.WinCondition > Configuration.GridSide)
            {
                errors.Add("WinCondition can't be larger than the grid's side length.");
            }

            if (Configuration.BoardSizeWidth < Configuration.GridSide)
            {
                errors.Add("The board's width can't be less than the grid's side length.");
            }
            
            if (Configuration.BoardSizeHeight < Configuration.GridSide)
            {
                errors.Add("The board's height can't be less than the grid's side length.");
            }
            if (Configuration.GridX < 0 || Configuration.GridX >= Configuration.BoardSizeWidth ||
                Configuration.GridX + (Configuration.GridSide / 2) > Configuration.BoardSizeWidth ||
                Configuration.GridX - (Configuration.GridSide / 2) < 0)
            {
                errors.Add("Change x coordinate so that the grid stays on the board.");
            }
            if (Configuration.GridY < 0 || Configuration.GridY >= Configuration.BoardSizeHeight ||
                Configuration.GridY + (Configuration.GridSide / 2) > Configuration.BoardSizeHeight ||
                Configuration.GridY - (Configuration.GridSide / 2) < 0)
            {
                errors.Add("Change y coordinate so that the grid stays on the board.");
            }
            
            if (!errors.IsNullOrEmpty())
            {
                return RedirectToPage("", new { errors = errors });
            }
            


            _configRepository.SaveConfig(Configuration);

            return RedirectToPage("./Index");
        }
    }
}