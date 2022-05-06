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
    public class AdministrationBookingModel : PageModel
    {
        public BookingData book { get; set; }

        private IAdministrationService _administrationService;
        
        public AdministrationBookingModel(IAdministrationService admin)
        {
            _administrationService = admin;
        }


        public void OnGet()
        {


        }



        public IActionResult OnPost()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (book.ResevertionId.Equals(0))
            {
                book.ResevertionId++;
            }

            return RedirectToPage("/AdministrationPages/AdministrationMineBookinger");
        }
    }
}
