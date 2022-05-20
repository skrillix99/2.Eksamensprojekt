using SuperBookerData;
using System;
using System.Collections.Generic;


namespace _2.Eksamensprojekt.Services.Interfaces
{
    public interface IUnderviserService
    {
        /// <summary>
        /// Opretter en ny reservations der bliver gemt i Reservations tabellen, som Underviser. Den regner TidSlut ud baseret på Dag og TidStart. Den henter BrugerID fra ILogIndService.
        /// </summary>
        /// <param name="newBooking">BookingData object. Skal have følgene TidStart, Dag og Bruger.BrugerEmail</param>
        void AddReservationUnderviser(BookingData newBooking);

        void BegrænsetAdgang(DateTime dag, int id, string email);
        bool CanDelete(DateTime dag);
    }
}