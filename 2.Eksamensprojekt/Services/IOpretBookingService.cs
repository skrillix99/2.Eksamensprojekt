using SuperBookerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2.Eksamensprojekt.Services
{
    public interface IOpretBookingService
    {
        public BookingData create(BookingData newBooking);
    }
}
