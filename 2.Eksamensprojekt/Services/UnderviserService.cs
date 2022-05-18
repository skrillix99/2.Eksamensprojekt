using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using _2.Eksamensprojekt.Services.Interfaces;
using SuperBookerData;

namespace _2.Eksamensprojekt.Services
{
    public class UnderviserService : IUnderviserService
    {
        private const string connectionString = "Data Source=zealandmarc.database.windows.net;Initial Catalog=SuperBooker4000;User ID=AdminMarc;Password=Marcus19;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private readonly IPersonService _logIndService;

        public UnderviserService(IPersonService logIndService)
        {
            _logIndService = logIndService;
        }
        /// <summary>
        /// Opretter en reservation og gemmer den i Databasen med de data BookingData objektet indeholder
        /// </summary>
        /// <param name="newBooking">Typen BookingData. Indeholder data om den nye reservation</param>
        public void AddReservationUnderviser(BookingData newBooking)
        {
            string sql = "insert into Reservation VALUES (@tidStart, @dag, 0, @brugerFK, @lokaleFK, @tidSlut, @bookesFor)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                TimeSpan tidSlut = newBooking.Dag.TimeOfDay.Add(newBooking.TidStart);
                int brugerID = _logIndService.GetSingelPersonByEmail(newBooking.BrugerEmail).BrugerID;

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@tidStart", newBooking.Dag.ToShortTimeString());
                cmd.Parameters.AddWithValue("@dag", newBooking.Dag.ToString("s"));
                cmd.Parameters.AddWithValue("@tidSlut", tidSlut);
                cmd.Parameters.AddWithValue("@brugerFK", brugerID);
                cmd.Parameters.AddWithValue("@lokaleFK", newBooking.Lokale.LokaleID);
                cmd.Parameters.AddWithValue("@bookesFor", (int)newBooking.BookesFor);

                cmd.Connection.Open();

                int rows = cmd.ExecuteNonQuery();

                if (rows != 1)
                {
                    throw new Exception("welp");
                }
            }

        }

        public bool CanDelete(DateTime dag, string email)
        {
            //DateTime dt = DateTime.Now.AddDays(-3); //TODO logic ændres           
            //if ((dag.Subtract(dt).Days <= 3))
            DateTime dt = DateTime.Now.AddDays(-3); //TODO logic ændres
            int newDay = dag.Subtract(dt).Days;
            if ((dag.Subtract(dt).Days <= 3))
            {
                throw new ArgumentOutOfRangeException("Må kun annullere med minimum 3 dages varsel.");
            }

            return true;
        }
        public void BegrænsetAdgang(DateTime dag, int id, string email)
        {

            if (id <= 0)
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