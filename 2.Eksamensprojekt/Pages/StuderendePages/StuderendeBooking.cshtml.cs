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
        private IBookingService _bookingService;

        [BindProperty]
        public BookingData Booking { get; set; }
        [BindProperty]
        public LokaleData Lokale { get; set; }

        public TimeSpan TidligstLovligeTid => (TimeSpan) _administrationService.GetAllStuderendeRettigheder()[3]; // henter den tidligste tid man må booke til
        public TimeSpan SenestLovligeTid => (TimeSpan) _administrationService.GetAllStuderendeRettigheder()[2]; // henter den seneste tid man må booke til

        public string ErrorMsg { get; set; }

        public StuderendeBookingModel(IStuderendeService studerendeService, IAdministrationService administrationService, 
            ILokalerService lokalerService, IBookingService bookingService)
        {
            _studerendeService = studerendeService;
            _administrationService = administrationService;
            _lokalerService = lokalerService;
            _bookingService = bookingService;

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
            if (Booking.BooketSmartBoard)
            {
                foreach (var sm in _bookingService.GetAllBookingsByIdAndDag(id, Booking.Dag))
                {
                    if (sm.BooketSmartBoard)
                    {
                        ErrorMsg = "Smartboardet er booket i forvejen";
                        return Page();
                    }
                }
            }


            Booking.TidSlut = Booking.TidStart.Add(Booking.TidSlut);

            //tjekker om man har booket før den lovligetid
            if (Booking.TidStart < TidligstLovligeTid)
            {
                ErrorMsg = $"Du må ikke booke før kl: {TidligstLovligeTid:hh\\:mm}";
                return Page();
            }

            TimeSpan bookingTilTid = Booking.TidSlut;
            try
            {
                //tjekker om man har booket til efter den tid man må booke til.
                if (bookingTilTid > SenestLovligeTid)
                {
                    ErrorMsg = $"Du må senest have et lokale booke til kl: {SenestLovligeTid:hh\\:mm}";
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
