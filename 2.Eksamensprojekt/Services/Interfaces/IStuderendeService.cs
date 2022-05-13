using System;
using System.Collections.Generic;
using SuperBookerData;

namespace _2.Eksamensprojekt.Services.Interfaces
{
    public interface IStuderendeService
    {
        string AddReservation(BookingData newBooking);
        LokaleData GetSingelLokale(int id);
        void DeleteReservation(int id);
        List<BookingData> GetAllReservationer(string sql2);
        BookingData GetSingelBooking(int id);
    }
}