using System;
using System.Collections.Generic;
using _2.Eksamensprojekt.Services.Interfaces;
using SuperBookerData;
using System.Data.SqlClient;
using Microsoft.IdentityModel.Claims;
using Claim = System.Security.Claims.Claim;

namespace _2.Eksamensprojekt.Services
{
    public class AdministrationService : IAdministrationService
    {
        private const string connectionString = "Data Source=zealandmarc.database.windows.net;Initial Catalog=SuperBooker4000;User ID=AdminMarc;Password=Marcus19;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private readonly IPersonService _logIndService;
        /// <summary>
        /// Laver dependency injection til at kunne bruge ILogIndService.
        /// </summary>
        /// <param name="logIndService">Typen ILogIndService</param>
        public AdministrationService(IPersonService logIndService)
        {
            _logIndService = logIndService;
        }

        /// <summary>
        /// Opretter en ny booking og gemmer den i databasen
        /// </summary>
        /// <param name="newBooking">Typen BookingData. Indeholder den nye bookings information</param>
        public void AddReservationAdmin(BookingData newBooking)
        {
            string sql = "insert into Reservation VALUES (@tidStart, @dag, @mulige, @brugerFK, @lokaleFK, @tidSlut, @bookesFor, 1)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                int brugerID = _logIndService.GetSingelPersonByEmail(newBooking.Bruger.BrugerEmail).BrugerID;

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@tidStart", newBooking.TidStart);
                cmd.Parameters.AddWithValue("@dag", newBooking.Dag.Date);
                cmd.Parameters.AddWithValue("@tidSlut", newBooking.TidSlut);
                cmd.Parameters.AddWithValue("@brugerFK", brugerID);
                cmd.Parameters.AddWithValue("@lokaleFK", newBooking.Lokale.LokaleID);
                cmd.Parameters.AddWithValue("@bookesFor", (int)newBooking.BookesFor);
                cmd.Parameters.AddWithValue("@mulige", newBooking.Lokale.MuligeBookinger);

                cmd.Connection.Open();

                int rows = cmd.ExecuteNonQuery();

                if (rows != 1)
                {
                    throw new Exception("welp");
                }
            }

        }

        public void StuderendeRettighederUpdate(int bookingLimit, TimeSpan senestBooking)
        {
            string sql = "UPDATE StuderendeRettigheder SET BookingLimit = @bookingLimit, SenestBookingTid = @senestBooking";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@bookingLimit", bookingLimit);
                cmd.Parameters.AddWithValue("@senestBooking", senestBooking);

                cmd.Connection.Open();

                int rows = cmd.ExecuteNonQuery();

                if (rows < 1)
                {
                    throw new ArgumentException("Der skete en fejl");
                }
            }
        }

        public void UpdateReservation(BookingData updatedBooking)
        {

            string sql = "UPDATE Reservation " +
                         "SET TidStart = @Tidstart, Dag = @dag, TidSlut = @Tidslut, BookesFor = @Bookesfor " +
                         "WHERE ReservationID = @id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);

                cmd.Parameters.AddWithValue("@Tidstart", updatedBooking.TidStart);
                cmd.Parameters.AddWithValue("@dag", updatedBooking.Dag);
                cmd.Parameters.AddWithValue("@Tidslut", updatedBooking.TidSlut);
                cmd.Parameters.AddWithValue("@Bookesfor", updatedBooking.BookesFor);
                cmd.Parameters.AddWithValue("@id", updatedBooking.ResevertionId);

                cmd.Connection.Open();

                int rows = cmd.ExecuteNonQuery();
                if (rows != 1)
                {
                    throw new InvalidOperationException("Der skete en fejl i databasen");
                }
            }
        }

        public List<object> GetAllStuderendeRettigheder()
        {
            List<object> objects = new List<object>();
            string sql = "select * from StuderendeRettigheder";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);

                cmd.Connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    int limit = reader.GetInt32(1);
                    TimeSpan senestBookingTid = reader.GetTimeSpan(2);
                    TimeSpan tidligstBookingTid = reader.GetTimeSpan(3);
                    objects.Add(id);
                    objects.Add(limit);
                    objects.Add(senestBookingTid);
                    objects.Add(tidligstBookingTid);
                }
            }

            return objects;
        }

    }
}