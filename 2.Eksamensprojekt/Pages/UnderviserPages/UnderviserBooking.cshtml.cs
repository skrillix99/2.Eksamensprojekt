using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2.Eksamensprojekt.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuperBookerData;

namespace _2.Eksamensprojekt.Pages.UnderviserPages
{
    [Authorize(Roles = "Underviser")]
    public class UnderviserBookingModel : PageModel
    {
        private IAdministrationService _administrationService;

        [BindProperty]
        public BookingData Booking { get; set; }
        [BindProperty]
        public PersonData Email { get; set; }
        [BindProperty]
        public LokaleData Lokale { get; set; }

        public UnderviserBookingModel(IAdministrationService administrationService)
        {
            _administrationService = administrationService;

            Email = new PersonData();
            Lokale = new LokaleData();
        }
        public void OnGet(int id)
        {
            Lokale = _administrationService.GetSingelLokale(id);
        }

        public void OnPost(int id)
        {
            Lokale = _administrationService.GetSingelLokale(id);
        }

        public void OnPostBook(int id)
        {
            Lokale.LokaleID = id;
            Booking.Lokale = Lokale;
            Booking.Bruger = Email;
            _administrationService.AddReservation(Booking);
        }
    }
}
