using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2.Eksamensprojekt.Services;
using _2.Eksamensprojekt.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuperBookerData;
using Microsoft.AspNetCore.Authorization;
// Alexander Gerdil
namespace _2.Eksamensprojekt.Pages.AdministrationPages
{
    [Authorize(Roles = "Administration")]
    public class AdministrationRedigerBookingModel : PageModel
    {
        private IBookingService _bookingService;
        private IAdministrationService _administrationService;

        [BindProperty]
        public BookingData Booking { get; set; }

        public string ErrorMsg { get; set; }

        public AdministrationRedigerBookingModel(IAdministrationService administrationService, IBookingService booking)
        {
            _administrationService = administrationService;
            _bookingService = booking;
            
        }
        
        public IActionResult OnGet(int id)
        {
            Booking = _bookingService.GetSingleBooking(id);
            

            return Page();
        }

        public IActionResult OnPost(int id)
        {

            if (Booking.TidStart > Booking.TidSlut)
            {
                ErrorMsg = "Til skal v?re senere end fra!";
                Booking = _bookingService.GetSingleBooking(id);
                return Page();
            }

            Booking.ResevertionId = id;
            _administrationService.UpdateReservation(Booking);
            return RedirectToPage("AdministrationMineBookinger");
        }

        
    }
}
