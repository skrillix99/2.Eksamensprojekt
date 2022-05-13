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
        private IAdministrationService _administrationService;

        public BookingData Booking { get; set; }

        public StuderendeAflysBookingModel(IStuderendeService studerendeService, IAdministrationService administrationService)
        {
            _studerendeService = studerendeService;
            _administrationService = administrationService;
        }
        public void OnGet(int id)
        {
            Booking = _administrationService.GetSingelBooking(id);
        }

        public void OnPost(int id)
        {
            Booking = _administrationService.GetSingelBooking(id);
            _studerendeService.DeleteReservation(id);
        }
    }
}
