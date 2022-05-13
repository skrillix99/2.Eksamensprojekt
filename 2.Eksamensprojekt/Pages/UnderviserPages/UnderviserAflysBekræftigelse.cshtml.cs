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
    public class UnderviserAflysBekræftigelseModel : PageModel
    {
        private IAdministrationService _administrationService;

        public BookingData Booking { get; set; }

        public UnderviserAflysBekræftigelseModel(IAdministrationService administrationService)
        {
            _administrationService = administrationService;
        }
        public void OnGet()
        {
        }
        public void OnPost(int id)
        {
            Booking = UnderviserAflysBookingModel.TempBookingData;
            _administrationService.DeleteReservation(id, Booking.Dag);
        }
    }
}
