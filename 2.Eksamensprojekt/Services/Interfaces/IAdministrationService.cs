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

        void DeleteResevation(int id);
        List<BookingData> GetAllReservationer();
        BookingData GetSingelBooking(int id);
    }
}