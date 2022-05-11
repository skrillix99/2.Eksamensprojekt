using System;
using System.Data.SqlClient;
using _2.Eksamensprojekt.Services.Interfaces;
using SuperBookerData;

namespace _2.Eksamensprojekt.Services
{
    public class UnderviserService: IUnderviserService
    {
        private const string connectionString = "Data Source=zealandmarc.database.windows.net;Initial Catalog=SuperBooker4000;User ID=AdminMarc;Password=Marcus19;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private ILogIndService _logIndService;

        public UnderviserService(ILogIndService logIndService)
        {
            _logIndService = logIndService;
        }

        public void AddReservation(BookingData newBooking)
        {
            string sql = "insert into Reservation VALUES (@tidStart, @dag, 0, @brugerFK, @lokaleFK, @tidSlut, 1)";
                                                        // TidStart, Dag, Heltbooket, Bruger_FK, Lokale_FK, TidSlut, BookesFor
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                TimeSpan tidSlut = newBooking.Dag.TimeOfDay.Add(newBooking.TidStart);
                int brugerID = _logIndService.GetSingelPersonByEmail(newBooking.BrugerEmail).BrugerID;

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@tidStart", newBooking.TidStart.ToString());
                cmd.Parameters.AddWithValue("@dag", newBooking.Dag.ToString("s"));
                cmd.Parameters.AddWithValue("@tidSlut", tidSlut.ToString());
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
    }
}