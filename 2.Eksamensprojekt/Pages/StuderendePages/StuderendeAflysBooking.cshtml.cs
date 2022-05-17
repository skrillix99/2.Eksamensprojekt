using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2.Eksamensprojekt.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuperBookerData;

namespace _2.Eksamensprojekt.Pages.StuderendePages
{
    [Authorize(Roles = "Student")]
    public class StuderendeAflysBookingModel : PageModel
    {
        private IStuderendeService _studerendeService;

        public BookingData Booking { get; set; }

        public static BookingData TempBookingData { get; set; }

        public StuderendeAflysBookingModel(IStuderendeService studerendeService)
        {
            _studerendeService = studerendeService;
        }
        public void OnGet(int id)
        {
            Booking = _studerendeService.GetSingelBooking(id);
            TempBookingData = _studerendeService.GetSingelBooking(id);
        }

        public void OnPost(int id)
        {
            Booking = _studerendeService.GetSingelBooking(id);
            TempBookingData = _studerendeService.GetSingelBooking(id);
        }
    }
}
