using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2.Eksamensprojekt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _2.Eksamensprojekt.Pages.AdministrationPages
{
    [Authorize(Roles = "Administration")]
    public class AdministrationAflysBookingModel : PageModel
    {
        private IBookingService _bookingService;

        public AdministrationAflysBookingModel(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }
        public void OnGet(int id)
        {

        }
    }
}
