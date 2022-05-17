using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using _2.Eksamensprojekt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuperBookerData;

namespace _2.Eksamensprojekt.Pages.Shared
{
    [Authorize(Roles = "Underviser")]
    [Authorize(Roles = "Administration")]

    public class BookedeLokalerModel : PageModel
    {

        private IBookingService _bookingService;

        public BookedeLokalerModel(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [BindProperty]
        public List<BookingData> BookingData { get; set; }

        public void OnGet()
        {
            BookingData = _bookingService.GetAllBookings();
        }
        public void OnPost()
        {
            BookingData = _bookingService.GetAllBookings();
        }
    }
}

