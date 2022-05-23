using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2.Eksamensprojekt.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _2.Eksamensprojekt.Pages.AdministrationPages
{
    [Authorize(Roles = "Administration")]
    public class AdministrationBegrænsningerModel : PageModel
    {
        private IAdministrationService _administrationService;
        [BindProperty]
        public int NewLimit { get; set; }
        [BindProperty]
        public TimeSpan SenestTid { get; set; }
        [BindProperty]
        public TimeSpan TidligstTid { get; set; }
        public string Msg { get; set; }

        public AdministrationBegrænsningerModel(IAdministrationService administrationService)
        {
            _administrationService = administrationService;
        }

        public void OnGet()
        {
            NewLimit = (int) _administrationService.GetAllStuderendeRettigheder()[1];
            SenestTid = (TimeSpan) _administrationService.GetAllStuderendeRettigheder()[2];
            TidligstTid = (TimeSpan) _administrationService.GetAllStuderendeRettigheder()[3];
        }

        public void OnPost()
        {
            Msg = "Opdateret";
            _administrationService.StuderendeRettighederUpdate(NewLimit, SenestTid, TidligstTid);
        }
    }
}
