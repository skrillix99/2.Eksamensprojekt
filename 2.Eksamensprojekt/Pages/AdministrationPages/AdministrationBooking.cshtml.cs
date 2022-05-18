using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2.Eksamensprojekt.Services;
using _2.Eksamensprojekt.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuperBookerData;

namespace _2.Eksamensprojekt.Pages.AdministrationPages
{
    public class AdministrationBookingModel : PageModel
    {
        private IAdministrationService _administrationService;
        private ILokalerService _lokalerService;

        [BindProperty]
        public BookingData Booking { get; set; }
        [BindProperty]
        public PersonData Email { get; set; }
        [BindProperty]
        public LokaleData Lokale { get; set; }

        public AdministrationBookingModel(IAdministrationService administrationService, ILokalerService lokalerService)
        {
            _administrationService = administrationService;
            _lokalerService = lokalerService;

            Email = new PersonData();
            Lokale = new LokaleData();
        }
        public void OnGet(int id)
        {
            Lokale = _lokalerService.GetSingelLokale(id);
        }

        public void OnPost(int id)
        {
            Lokale = _lokalerService.GetSingelLokale(id);


        }

        public IActionResult OnPostBook(int id)
        {
            Lokale.LokaleID = id;
            Booking.Lokale = Lokale;
            Booking.Bruger = Email;
            _administrationService.AddReservation(Booking);

            return RedirectToPage("/Shared/LedigeLokaler");
        }



    }
}
