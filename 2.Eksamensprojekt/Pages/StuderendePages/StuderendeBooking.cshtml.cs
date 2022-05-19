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

namespace _2.Eksamensprojekt.Pages.StuderendePages
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

        public TimeSpan SenestBooketTid => (TimeSpan) _administrationService.GetAllStuderendeRettigheder()[2];

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
            TimeSpan senestTid = (TimeSpan) _administrationService.GetAllStuderendeRettigheder()[2];
            TimeSpan slutBookingTid = Booking.Dag.TimeOfDay.Add(Booking.TidStart);
            try
            {
                if (slutBookingTid > senestTid)
                {
                    ErrorMsg = $"Du må senest have et lokale booke til kl: {senestTid:hh\\:mm}";
                    return Page();
                }


                Lokale.LokaleID = id;
                Booking.Lokale = _lokalerService.GetSingelLokale(id);
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
