using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using _2.Eksamensprojekt.Services;
using _2.Eksamensprojekt.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuperBookerData;

namespace _2.Eksamensprojekt.Pages.StuderendePages // Marcus
{
    [Authorize(Roles = "Student")]
    public class StuderendeBookingModel : PageModel
    {
        private IStuderendeService _studerendeService;
        private IAdministrationService _administrationService;
        private ILokalerService _lokalerService;

        [BindProperty]
        public BookingData Booking { get; set; }
        [BindProperty]
        public LokaleData Lokale { get; set; }

        public TimeSpan SenestBooketTid => (TimeSpan) _administrationService.GetAllStuderendeRettigheder()[2]; // henter den seneste tid man m� booke til

        public string ErrorMsg { get; set; }

        public StuderendeBookingModel(IStuderendeService studerendeService, IAdministrationService administrationService, ILokalerService lokalerService)
        {
            _studerendeService = studerendeService;
            _administrationService = administrationService;
            _lokalerService = lokalerService;

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

            //tjekker om man har booket f�r kl:08:00
            if (Booking.TidStart < TimeSpan.Parse("08:00"))
            {
                ErrorMsg = "Du m� ikke booke f�r kl: 08:00";
                return Page();
            }
            
            //tjekker hvis man har valgt et m�delokale 
            if (Lokale.LokaleSize == LokaleSize.M�delokale)
            {
                Booking.TidSlut = Booking.TidStart.Add(TimeSpan.FromHours(2));
                Booking.BooketSmartBoard = true;
            }

            //tjekker om man har valgt at slut tiden er f�r start tiden p� en booking
            if (Booking.TidStart > Booking.TidSlut)
            {
                ErrorMsg = "Til skal v�re senere end Fra!";
                return Page();
            }

            TimeSpan senestLovligeTid = (TimeSpan) _administrationService.GetAllStuderendeRettigheder()[2];
            TimeSpan bookingTilTid = Booking.TidSlut;
            try
            {
                //tjekker om man har booket til efter den tid man m� booke til.
                if (bookingTilTid > senestLovligeTid)
                {
                    ErrorMsg = $"Du m� senest have et lokale booke til kl: {senestLovligeTid:hh\\:mm}";
                    return Page();
                }


                Lokale.LokaleID = id;
                Booking.Lokale = Lokale;
                _studerendeService.AddReservation(Booking);
            }
            catch (ArgumentOutOfRangeException e)
            {
                ErrorMsg = e.ParamName;
                return Page();
            }

            return RedirectToPage("StuderendeMineBookinger");
        }
    }
}
