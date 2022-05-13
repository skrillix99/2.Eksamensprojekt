using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SuperBookerData;

namespace _2.Eksamensprojekt.Services.Interfaces
{
    public interface IAdministrationService
    {
        /// <summary>
        /// Henter alle lokaler der ligger i Lokale tabel.
        /// </summary>
        /// <returns>List af type LokaleData</returns>
        List<LokaleData> GetAllLokaler();
        /// <summary>
        /// Henter et enkelt lokale fra Lokale tabel, baseret på det parameter der bliver sendt med.
        /// </summary>
        /// <param name="id">Typen int. Må ikke være negativt</param>
        /// <returns>et object af typen LokaleData</returns>
        LokaleData GetSingelLokale(int id);
        /// <summary>
        /// Sletter en reservation fra Reservations tabellen.
        /// </summary>
        /// <param name="id">Typen int. Skal passe med et reservations id</param>
        /// <param name="dag">Typen DateTime. Skal være DateTime objectet da resevertionen blev oprettet</param>
        void DeleteReservation(int id);
        void DeleteReservation();
        List<BookingData> GetAllReservationer(string sql2);
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


    }
}