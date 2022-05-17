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
    public class StuderendeAflysBekræftigelseModel : PageModel
    {
        private IAdministrationService _administrationService;

        public BookingData Booking { get; set; }

        public StuderendeAflysBekræftigelseModel(IAdministrationService administrationService)
        {
            _administrationService = administrationService;
        }

        public void OnGet()
        {
        }

        public void OnPost(int id)
        {
            Booking = StuderendeAflysBookingModel.TempBookingData;
        }

        public void OnPostDel(int id)
        {
            _administrationService.DeleteReservation(id);
        }
    }
}
