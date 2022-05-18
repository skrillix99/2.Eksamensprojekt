using SuperBookerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace _2.Eksamensprojekt.Services
{
    public interface IBookingService
    {
        List<BookingData> GetAllBookings();
        List<BookingData> GetAllReservationerByRolle(string sql2);

        void DeleteResevation(int id);

        //TODO public BookingData CreateBooking(????);

    }
}