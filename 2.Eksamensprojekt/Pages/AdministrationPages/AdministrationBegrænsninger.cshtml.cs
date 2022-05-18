using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2.Eksamensprojekt.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _2.Eksamensprojekt.Pages.AdministrationPages
{
    public class AdministrationBegrænsningerModel : PageModel
    {
        private IAdministrationService _administrationService;
        [BindProperty]
        public int NewLimit { get; set; }
        [BindProperty]
        public TimeSpan TimeLimit { get; set; }

        public AdministrationBegrænsningerModel(IAdministrationService administrationService)
        {
            _administrationService = administrationService;
        }

        public void OnGet()
        {
            NewLimit = (int) _administrationService.GetAllStuderendeRettigheder()[1];
            TimeLimit = (TimeSpan) _administrationService.GetAllStuderendeRettigheder()[2];
        }

        public void OnPost()
        {
            _administrationService.StuderendeRettighederUpdate(NewLimit, TimeLimit);
        }
    }
}
