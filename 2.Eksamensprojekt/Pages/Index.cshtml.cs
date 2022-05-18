using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2.Eksamensprojekt.Pages.LogInd;
using _2.Eksamensprojekt.Services;
using _2.Eksamensprojekt.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace _2.Eksamensprojekt.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private IBookingService _bookingService;

        public IndexModel(ILogger<IndexModel> logger, IBookingService bookingService)
        {
            _logger = logger;
            _bookingService = bookingService;
        }

        public void OnGet()
        {
            _bookingService.DeleteReservationByDay();

            if (LogIndModel.LoggedInUser == null)
            {
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            
        }

    }
}
