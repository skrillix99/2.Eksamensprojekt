using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2.Eksamensprojekt.Services;
using Microsoft.AspNetCore.Authorization;
using _2.Eksamensprojekt.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuperBookerData;

namespace _2.Eksamensprojekt.Pages.AdministrationPages
{
    [Authorize(Roles = "Administration")]
    public class AdministrationAflysBookingModel : PageModel
    {
        private IAdministrationService _administrationService;
        private IBookingService _bookingService;

        public BookingData Booking { get; set; }
        public static BookingData TempBookingData { get; set; }

        public AdministrationAflysBookingModel(IAdministrationService administrationService, IBookingService bookingService)
        {
            _administrationService = administrationService;
            _bookingService = bookingService;
        }

        public void OnGet(int id)
        {
            Booking = _administrationService.GetSingelBooking(id);
            TempBookingData = _administrationService.GetSingelBooking(id);
        }
        
    }
}
