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
    public class UnderviserAflysBookingModel : PageModel
    {
        private IAdministrationService _administrationService;

        public BookingData Booking { get; set; }
        public static BookingData TempBookingData { get; set; }

        public UnderviserAflysBookingModel(IAdministrationService administrationService)
        {
            _administrationService = administrationService;
        }
        public void OnGet(int id)
        {
            Booking = _administrationService.GetSingelBooking(id);
            TempBookingData = _administrationService.GetSingelBooking(id);
        }
        public void OnPost(int id)
        {
            Booking = _administrationService.GetSingelBooking(id);
            TempBookingData = _administrationService.GetSingelBooking(id);
        }
    }
}
