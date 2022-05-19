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
    public class UnderviserBookingModel : PageModel
    {
        private IUnderviserService _underviserService;
        private ILokalerService _lokalerService;

        [BindProperty]
        public BookingData Booking { get; set; }
        [BindProperty]
        public LokaleData Lokale { get; set; }
        public string ErrorMsg { get; set; }

        public UnderviserBookingModel(IUnderviserService underviserService, ILokalerService lokalerService)
        {
            _underviserService = underviserService;
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
            if (Booking.TidStart > Booking.TidSlut)
            {
                ErrorMsg = "Til skal være senere end fra!";
                return Page();
            }

            Lokale.LokaleID = id;
            Booking.Lokale = Lokale;
            _underviserService.AddReservationUnderviser(Booking);
            
            return RedirectToPage("UnderviserMineBookinger");
        }
    }
}
