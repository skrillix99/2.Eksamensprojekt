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

namespace _2.Eksamensprojekt.Pages.AdministrationPages
{
    [Authorize(Roles = "Administration")]
    public class AdministrationAflysBekræftigelseModel : PageModel
    {
        private IBookingService _bookingService;

        public BookingData Booking{ get; set; }

        public AdministrationAflysBekræftigelseModel(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public void OnGet()
        {
        }

        public void OnPost(int id)
        {
            Booking = AdministrationAflysBookingModel.TempBookingData;
        }

        public void OnPostDel(int id)
        {
            _bookingService.DeleteReservationById(id);
        }
    }
}
