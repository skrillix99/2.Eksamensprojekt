using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2.Eksamensprojekt.Pages.AdministrationPages;
using _2.Eksamensprojekt.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuperBookerData;

namespace _2.Eksamensprojekt.Pages.UnderviserPages
{
    [Authorize(Roles = "Underviser")]
    public class UnderviserAflysBekræftigelseModel : PageModel // Marcus
    {
        private IUnderviserService _underviserService;

        public BookingData Booking { get; set; }
        public string ErrorMsg { get; set; }
        public UnderviserAflysBekræftigelseModel(IUnderviserService underviserService)
        {
            _underviserService = underviserService;
        }
        public void OnGet()
        {
            Booking = UnderviserAflysBookingModel.TempBookingData;
        }

        public IActionResult OnPost(int id)
        {
            try
            {
                Booking = UnderviserAflysBookingModel.TempBookingData;
                _underviserService.BegrænsetAdgang(Booking.Dag, id);
            }
            catch (ArgumentOutOfRangeException e)
            {
                ErrorMsg = e.ParamName;
                return Page();
            }

            return RedirectToPage("UnderviserMineBookinger");
        }
    }
}
