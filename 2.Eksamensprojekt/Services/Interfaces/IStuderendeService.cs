using System;
using System.Collections.Generic;
using SuperBookerData;

namespace _2.Eksamensprojekt.Services.Interfaces
{
    public interface IStuderendeService
    {
        string AddReservation(BookingData newBooking);
        void DeleteReservation(int id);
        BookingData GetSingleBookingByIdAndDay(int id, DateTime dag);
    }
}