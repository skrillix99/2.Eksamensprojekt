using SuperBookerData;
using System;
using System.Collections.Generic;


namespace _2.Eksamensprojekt.Services.Interfaces
{
    public interface IUnderviserService
    {
        void AddReservationUnderviser(BookingData newBooking);
        void BegrænsetAdgang(DateTime dag, int id);
        bool CanDelete(DateTime dag);
    }
}