using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using _2.Eksamensprojekt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuperBookerData;

namespace _2.Eksamensprojekt.Pages.Shared
{
    public class BookedeLokalerModel : PageModel
    {

        private IBookingService _bookingService;

        public BookedeLokalerModel(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [BindProperty]
        public LokaleData LokaleData { get; set; }

        public void OnGet(int LokaleID)
        {
            LokaleData = _bookingService.GetById(LokaleID);
        }


        public IActionResult OnPost(int LokaleID)
        {
            LokaleData deletedLokale = _bookingService.Delete(LokaleID);

            return RedirectToPage("Index");
        }

    }
}

