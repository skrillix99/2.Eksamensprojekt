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
    public class StuderendeAflysBookingModel : PageModel // Marcus
    {
        private IStuderendeService _studerendeService;
        private IBookingService _bookingService;
        public BookingData Booking { get; set; }

        public static BookingData TempBookingData { get; set; }

        public StuderendeAflysBookingModel(IStuderendeService studerendeService, IBookingService bookingService)
        {
            _studerendeService = studerendeService;
            _bookingService = bookingService;
        }
        public void OnGet(int id)
        {
            Booking = _bookingService.GetSingleBooking(id);
            TempBookingData = _bookingService.GetSingleBooking(id);
        }

        public void OnPost(int id)
        {
            Booking = _bookingService.GetSingleBooking(id);
            TempBookingData = _bookingService.GetSingleBooking(id);
        }
    }
}
