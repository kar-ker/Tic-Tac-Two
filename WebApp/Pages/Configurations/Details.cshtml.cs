using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages_Configurations
{
    public class DetailsModel : PageModel
    {
        private readonly IConfigRepository _configRepository;

        public DetailsModel(IConfigRepository configRepository)
        {
            _configRepository = configRepository;
        }

        [BindProperty]
        public Configuration Configuration { get; set; } = default!;
        
        public IActionResult OnGet(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Configuration = _configRepository.GetConfigurationById(id);
            return Page();
        }
        public  IActionResult OnPost(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var status = _configRepository.DeleteConfiguration(id);
            if (status != "success")
            {
                return NotFound();
            }

            return RedirectToPage("./Index");
        }
    }
}
