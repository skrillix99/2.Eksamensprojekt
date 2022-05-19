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
        /// <summary>
        /// Henter en bestemt reservation fra Rerservations tabellen, baseret på parameter id.
        /// </summary>
        /// <param name="id">Typen int. Skal passe med et reservations id</param>
        /// <returns>BookingData objet</returns>
        BookingData GetSingleBooking(int id);
        /// <summary>
        /// Sletter en reservation fra Reservations tabellen.
        /// </summary>
        /// <param name="id">Typen int. Skal passe med et reservations id</param>
        void DeleteReservationById(int id);
        /// <summary>
        /// Sletter alle bookinger fra dagen før. 
        /// </summary>
        void DeleteReservationByDay();

    }
}