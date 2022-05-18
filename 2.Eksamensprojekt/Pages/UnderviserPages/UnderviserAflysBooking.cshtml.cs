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

namespace _2.Eksamensprojekt.Pages.UnderviserPages
{
    [Authorize(Roles = "Underviser")]
    public class UnderviserAflysBookingModel : PageModel
    {
        private readonly IAdministrationService _administrationService;
        private readonly IUnderviserService _underviserService;
        public BookingData Booking { get; set; }
        public PersonData Bruger { get; set; }
        public static BookingData TempBookingData { get; set; }
        public string ErrorMsg { get; private set; }

        public UnderviserAflysBookingModel(IAdministrationService administrationService, IUnderviserService underviserService)
        {
            _administrationService = administrationService;
            _underviserService = underviserService;
            Booking = new BookingData();
            Bruger = new PersonData();
        }
        public void OnGet(int id)
        {
            Booking = _administrationService.GetSingelBooking(id);
            TempBookingData = _administrationService.GetSingelBooking(id);
        }
        public void OnPost(int id)
        {
            try
            {
                foreach (var userClaim in User.Claims)
                {
                    if (userClaim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")
                    {
                        Bruger.BrugerEmail = userClaim.Value;
                    }
                }
                
                    Booking = _administrationService.GetSingelBooking(id);
                TempBookingData = _administrationService.GetSingelBooking(id);
                //_underviserService.CanDelete(Booking.Dag, Bruger.BrugerEmail);
            }
            catch (ArgumentOutOfRangeException e)
            {
                ErrorMsg = e.ParamName;
            }

        }
    }
}
