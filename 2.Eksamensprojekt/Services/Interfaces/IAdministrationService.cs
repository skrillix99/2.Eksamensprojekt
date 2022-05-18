using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SuperBookerData;

namespace _2.Eksamensprojekt.Services.Interfaces
{
    public interface IAdministrationService
    {
        
        /// <summary>
        /// Opretter en ny reservations der bliver gemt i Reservations tabellen, som Administration. Den regner TidSlut ud baseret på Dag og TidStart. Den henter BrugerID fra ILogIndService.
        /// </summary>
        /// <param name="newBooking">BookingData object. Skal have følgene TidStart, Dag, Bruger.BrugerEmail og BookesFor</param>
        void AddReservationAdmin(BookingData newBooking);

        void UpdateReservation(BookingData updatedBooking);


        void StuderendeRettighederUpdate(int bookingLimit, TimeSpan senestBooking);
        List<object> GetAllStuderendeRettigheder();
    }
}