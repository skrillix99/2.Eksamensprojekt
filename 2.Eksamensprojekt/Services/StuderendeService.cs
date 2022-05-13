using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using _2.Eksamensprojekt.Services.Interfaces;
using SuperBookerData;

namespace _2.Eksamensprojekt.Services
{
    public class StuderendeService: IStuderendeService
    {
        private const string connectionString = "Data Source=zealandmarc.database.windows.net;Initial Catalog=SuperBooker4000;User ID=AdminMarc;Password=Marcus19;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private ILogIndService _logIndService;
        /// <summary>
        /// Laver dependency injection til at kunne bruge ILogIndService.
        /// </summary>
        /// <param name="logIndService">Typen ILogIndService</param>
        public StuderendeService(ILogIndService logIndService)
        {
            _logIndService = logIndService;
        }


        public void AddReservation(BookingData newBooking)
        {
            string sql = "insert into Reservation VALUES (@tidStart, @dag, @mulige, @brugerFK, @lokaleFK, @tidSlut, 0)";
            // TidStart, Dag, Heltbooket, Bruger_FK, Lokale_FK, TidSlut, BookesFor
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string tidSlut = newBooking.Dag.Add(newBooking.TidStart).ToShortTimeString();
                int brugerID = _logIndService.GetSingelPersonByEmail(newBooking.Bruger.BrugerEmail).BrugerID;

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@tidStart", newBooking.Dag.ToShortTimeString());
                cmd.Parameters.AddWithValue("@dag", newBooking.Dag.ToString("s"));
                cmd.Parameters.AddWithValue("@mulige", newBooking.Dag.ToString("s"));
                cmd.Parameters.AddWithValue("@tidSlut", tidSlut);
                cmd.Parameters.AddWithValue("@brugerFK", brugerID);
                cmd.Parameters.AddWithValue("@lokaleFK", newBooking.Lokale.LokaleID);

                cmd.Connection.Open();

                int rows = cmd.ExecuteNonQuery();

                if (rows != 1)
                {
                    throw new Exception("welp");
                }
            }
        }

        public LokaleData GetSingelLokale(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteReservation(int id, DateTime dag)
        {
            throw new NotImplementedException();
        }

        public List<BookingData> GetAllReservationer(string sql2)
        {
            throw new NotImplementedException();
        }

        public BookingData GetSingelBooking(int id)
        {
            throw new NotImplementedException();
        }
    }
}