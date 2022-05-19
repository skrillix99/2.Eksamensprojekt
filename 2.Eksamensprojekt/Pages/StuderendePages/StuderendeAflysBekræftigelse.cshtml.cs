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
    public class StuderendeAflysBekræftigelseModel : PageModel
    {
        private IStuderendeService _studerendeService;
        private IBookingService _bookingService;

        public BookingData Booking { get; set; }
        public BookingData TempBookingData { get; set; }

        public StuderendeAflysBekræftigelseModel(IStuderendeService studerendeService, IBookingService bookingService)
        {
            _studerendeService = studerendeService;
            _bookingService = bookingService;
        }

        public void OnGet(int id)
        {
            TempBookingData = _bookingService.GetSingleBooking(id);
        }

        public void OnPost(int id)
        {
            Booking = StuderendeAflysBookingModel.TempBookingData;
        }

        public IActionResult OnPostDel(int id)
        {
            _studerendeService.DeleteReservation(id);

            return RedirectToPage("StuderendeMineBookinger");
        }
    }
}
