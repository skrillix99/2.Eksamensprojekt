using System;
using _2.Eksamensprojekt.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuperBookerData;

namespace _2.Eksamensprojekt.Pages.AdministrationPages
{
    public class AdministrationBookingModel : PageModel
    {
        [BindProperty]
        public BookingData book { get; set; }

        private IAdministrationService _administrationService;
        
        public AdministrationBookingModel(IAdministrationService admin)
        {
            _administrationService = admin;
        }


        public void OnGet()
        {


        }



    }
}
