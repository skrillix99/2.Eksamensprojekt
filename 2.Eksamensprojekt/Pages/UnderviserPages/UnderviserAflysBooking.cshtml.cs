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
    public class UnderviserAflysBookingModel : PageModel // Marcus
    {
        private readonly IBookingService _bookingService;
        private readonly IUnderviserService _underviserService;
        public BookingData Booking { get; set; }
        public PersonData Bruger { get; set; }
        public static BookingData TempBookingData { get; set; }
        public string ErrorMsg { get; private set; }

        public UnderviserAflysBookingModel(IBookingService bookingService, IUnderviserService underviserService)
        {
            _bookingService = bookingService;
            _underviserService = underviserService;
            Booking = new BookingData();
            Bruger = new PersonData();
        }
        public void OnGet(int id)
        {
            
            Booking = _bookingService.GetSingleBooking(id);
            TempBookingData = _bookingService.GetSingleBooking(id);
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
                
                Booking = _bookingService.GetSingleBooking(id);
                TempBookingData = _bookingService.GetSingleBooking(id);
                _underviserService.CanDelete(Booking.Dag);
            }
            catch (ArgumentOutOfRangeException e)
            {
                ErrorMsg = e.ParamName;
            }

        }
    }
}
