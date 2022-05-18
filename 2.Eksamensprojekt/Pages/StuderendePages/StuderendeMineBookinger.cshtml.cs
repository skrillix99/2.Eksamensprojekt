using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2.Eksamensprojekt.Services;
using _2.Eksamensprojekt.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuperBookerData;

namespace _2.Eksamensprojekt.Pages.StuderendePages
{
    public class StuderendeMineBookingerModel : PageModel
    {
        private IBookingService _bookingService;

        public List<BookingData> Booking { get; private set; }

        public StuderendeMineBookingerModel(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }
        public void OnGet()
        {
            string sql2 = "where BrugerRolle = 0";

            Booking = _bookingService.GetAllReservationerByRolle(sql2);
        }
        public void OnPost()
        {
            string sql2 = "where BrugerRolle = 0";

            Booking = _bookingService.GetAllReservationerByRolle(sql2);
        }
    }
}
