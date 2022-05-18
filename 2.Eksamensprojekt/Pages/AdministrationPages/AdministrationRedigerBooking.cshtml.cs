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
using _2.Eksamensprojekt.Services;

namespace _2.Eksamensprojekt.Pages.AdministrationPages
{
    [Authorize(Roles = "Administration")]
    public class AdministrationRedigerBookingModel : PageModel
    {
        private IBookingService _bookingService;
        private IAdministrationService _administrationService;
        private IBookingService _bookingService;
        
        [BindProperty]
        public BookingData Booking { get; set; }


<<<<<<< HEAD
        public AdministrationRedigerBookingModel(IAdministrationService administrationService, IBookingService booking)
=======
        public AdministrationRedigerBookingModel(IBookingService bookingService, IAdministrationService administrationService)
>>>>>>> a373301ed9aee7c36ff8defbc9eeed1cd549f711
        {
            _bookingService = bookingService;
            _administrationService = administrationService;
<<<<<<< HEAD
            _bookingService = booking;
            
=======


>>>>>>> a373301ed9aee7c36ff8defbc9eeed1cd549f711
        }
        
        public IActionResult OnGet(int id)
        {
            Booking = _bookingService.GetSingleBooking(id);
            

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            Booking.ResevertionId = id;
            Booking.TidStart = Booking.Dag.TimeOfDay;
            _administrationService.UpdateReservation(Booking);
            return RedirectToPage("AdministrationMineBookinger");
        }

        
    }
}
