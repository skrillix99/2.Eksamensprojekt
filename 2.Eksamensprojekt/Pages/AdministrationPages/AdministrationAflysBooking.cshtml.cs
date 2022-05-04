using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2.Eksamensprojekt.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuperBookerData;

namespace _2.Eksamensprojekt.Pages.AdministrationPages
{
    public class AdministrationAflysBookingModel : PageModel
    {
        private IAdministrationService _administrationService;

        public LokaleData Lokale { get; set; }
        public int test { get; set; }

        public AdministrationAflysBookingModel(IAdministrationService administrationService)
        {
            _administrationService = administrationService;
        }

        public void OnGet(int id)
        {
            test = id;
            Lokale = _administrationService.GetSingelLokale(id);
        }
    }
}
