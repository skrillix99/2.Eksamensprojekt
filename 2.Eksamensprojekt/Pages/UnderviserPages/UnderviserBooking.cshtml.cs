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
using System.Threading;

namespace _2.Eksamensprojekt.Pages.UnderviserPages
{
    [Authorize(Roles = "Underviser")]
    public class UnderviserBookingModel : PageModel // Marcus
    {
        private IUnderviserService _underviserService;
        private ILokalerService _lokalerService;
        private IAdministrationService _administrationService;

        [BindProperty]
        public BookingData Booking { get; set; }
        [BindProperty]
        public LokaleData Lokale { get; set; }
        public static int CountUp { get; set; }
        public TimeSpan TidligstLovligeTid => (TimeSpan)_administrationService.GetAllStuderendeRettigheder()[3]; // henter den tidligste tid man må booke til
        public string ErrorMsg { get; set; }

        public UnderviserBookingModel(IUnderviserService underviserService, ILokalerService lokalerService, IAdministrationService administrationService)
        {
            _underviserService = underviserService;
            _lokalerService = lokalerService;
            _administrationService = administrationService;

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
            Lokale = _lokalerService.GetSingelLokale(id);

            if (Booking.TidStart < TidligstLovligeTid)
            {
                ErrorMsg = $"Du må ikke booke før kl: {TidligstLovligeTid:hh\\:mm}";
                return Page();
            }

            if (Booking.TidStart > Booking.TidSlut)
            {
                ErrorMsg = "Til skal være senere end fra!";
                return Page();
            }

            if (CountUp == 0)
            {
                Lokale.LokaleID = id;
                Booking.Lokale = Lokale;
                _underviserService.AddReservationUnderviser(Booking);
            }
            CountUp++;
            
            return RedirectToPage("UnderviserMineBookinger");
        }
    }
}
