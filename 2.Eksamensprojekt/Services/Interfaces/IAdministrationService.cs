using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SuperBookerData;

namespace _2.Eksamensprojekt.Services.Interfaces
{
    public interface IAdministrationService
    {
        /// <summary>
        /// Sletter en reservation fra Reservations tabellen.
        /// </summary>
        /// <param name="id">Typen int. Skal passe med et reservations id</param>
        /// <param name="dag">Typen DateTime. Skal være DateTime objectet da resevertionen blev oprettet</param>
        void DeleteReservationById(int id);
        void DeleteReservationByDay();
        /// <summary>
        /// Henter en bestemt reservation fra Rerservations tabellen, baseret på parameter id.
        /// </summary>
        /// <param name="id">Typen int. Skal passe med et reservations id</param>
        /// <returns>BookingData objet</returns>
        BookingData GetSingelBooking(int id);
        /// <summary>
        /// Opretter en ny reservations der bliver gemt i Reservations tabellen, som Administration. Den regner TidSlut ud baseret på Dag og TidStart. Den henter BrugerID fra ILogIndService.
        /// </summary>
        /// <param name="newBooking">BookingData object. Skal have følgene TidStart, Dag, Bruger.BrugerEmail og BookesFor</param>
        void AddReservation(BookingData newBooking);

        void StuderendeRettighederUpdate(int bookingLimit, TimeSpan senestBooking);
        List<object> GetAllStuderendeRettigheder();
    }
}