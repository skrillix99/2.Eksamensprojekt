using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using _2.Eksamensprojekt.Services.Interfaces;
using SuperBookerData;

namespace _2.Eksamensprojekt.Services // Marcus
{
    public class UnderviserService : IUnderviserService
    {
        private const string connectionString = "Data Source=zealandmarc.database.windows.net;Initial Catalog=SuperBooker4000;User ID=AdminMarc;Password=Marcus19;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private readonly IPersonService _logIndService;
        private readonly IBookingService _bookingService;

        public UnderviserService(IPersonService logIndService, IBookingService bookingService)
        {
            _logIndService = logIndService;
            _bookingService = bookingService;
        }
        /// <summary>
        /// Opretter en reservation og gemmer den i Databasen med de data BookingData objektet indeholder. Den henter BrugerID fra ILogIndService.
        /// </summary>
        /// <param name="newBooking">Typen BookingData. Skal indeholder data om den nye reservation</param>
        public void AddReservationUnderviser(BookingData newBooking)
        {
            string sql = "insert into Reservation VALUES (@tidStart, @dag, 0, @brugerFK, @lokaleFK, @tidSlut, @bookesFor, 1)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                if (newBooking.Lokale.LokaleID == 0)
                {
                    newBooking.Lokale.LokaleID = _bookingService.GetSingleBooking(newBooking.ResevertionId).Lokale.LokaleID;
                }

                int brugerID = _logIndService.GetSingelPersonByEmail(newBooking.BrugerEmail).BrugerID;

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@tidStart", newBooking.TidStart);
                cmd.Parameters.AddWithValue("@dag", newBooking.Dag.Date);
                cmd.Parameters.AddWithValue("@tidSlut", newBooking.TidSlut);
                cmd.Parameters.AddWithValue("@brugerFK", brugerID);
                cmd.Parameters.AddWithValue("@lokaleFK", newBooking.Lokale.LokaleID);
                cmd.Parameters.AddWithValue("@bookesFor", (int)newBooking.BookesFor);

                cmd.Connection.Open();

                int rows = cmd.ExecuteNonQuery();

                if (rows != 1)
                {
                    throw new Exception("Der skete en fejl med databasen. prøv senere");
                }
            }

        }

        /// <summary>
        /// Finder ud af om en underviser har lov til at slette en reservation baseret på hvor mange dage der er gået.
        /// Hvis ikke så throwner den en exception
        /// </summary>
        /// <param name="dag">typen DateTime. skal indeholde dagen reservationen er fra.</param>
        /// <returns>True</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public bool CanDelete(DateTime dag)
        {
            DateTime dt = DateTime.Now.Date;
            if (dag.Subtract(dt).Days < 3)
            {
                throw new ArgumentOutOfRangeException("Må kun annullere med minimum 3 dages varsel.");
            }

            return true;
        }

        /// <summary>
        /// Sletter en reservation hvis CanDelete() returner TRUE. Den sletter baseret på det ID der bliver sendt via parameter
        /// </summary>
        /// <param name="dag">Typen DateTime. Skal indeholde dagen reservationen er fra. </param>
        /// <param name="id">Typen Int. skal være et gyldt reservations id.</param>
        /// <exception cref="KeyNotFoundException"></exception>
        public void BegrænsetAdgang(DateTime dag, int id)
        {
            CanDelete(dag);

            if (_bookingService.GetSingleBooking(id).ResevertionId != id)
            {
                throw new KeyNotFoundException("Der findes ikke nogle reservationer med det ID");
            }

            string sql = "DELETE from Reservation WHERE ReservationID = @id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.Connection.Open();

                int rows = cmd.ExecuteNonQuery();
                if (rows != 1)
                {
                    throw new InvalidOperationException("Der skete en fejl i databasen");
                }
            }
        }
    }
}