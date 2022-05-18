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
        private readonly ILogIndService _logIndService;
        /// <summary>
        /// Laver dependency injection til at kunne bruge ILogIndService.
        /// </summary>
        /// <param name="logIndService">Typen ILogIndService</param>
        public AdministrationService(ILogIndService logIndService)
        {
            _logIndService = logIndService;
        }


        #region ReadLokale

        /// <summary>
        /// Oversætter data fra et Lokale database kald til et LokaleData object med alle columns.
        /// </summary>
        /// <param name="reader">Typen SqlDataReader. object med data fra database kald</param>
        /// <returns>et object af typen LokaleData</returns>
        public LokaleData ReadLokale(SqlDataReader reader)
        {
            LokaleData l = new LokaleData();

            l.LokaleID = reader.GetInt32(0);
            l.LokaleNavn = reader.GetString(1);
            l.LokaleSmartBoard = reader.GetBoolean(2);
            l.LokaleSize = (LokaleSize)reader.GetInt32(7);
            l.LokaleNummer = reader.GetString(10);
            l.MuligeBookinger = reader.GetInt32(8);
            l.Etage = reader.GetInt32(11);

            return l;
        }

        #endregion

        #region ReadReservation
        /// <summary>
        /// Oversætter data fra et Booking database kald til et BookingData object med alle columns.
        /// </summary>
        /// <param name="reader">Typen SqlDataReader. object med data fra database kald</param>
        /// <returns>et object af typen BookingData</returns>
        private BookingData ReadReservation(SqlDataReader reader)
        {
            BookingData b = new BookingData();
            b.ResevertionId = reader.GetInt32(0);
            b.TidStart = reader.GetTimeSpan(1);
            b.Dag = reader.GetDateTime(2);
            b.HeltBooket = reader.GetInt32(3);
            b.TidSlut = reader.GetTimeSpan(4);
            b.BookesFor = (brugerRolle)reader.GetInt32(5);
            b.brugerRolle = (brugerRolle)reader.GetInt32(6);
            b.BrugerID = reader.GetInt32(7);
            b.BrugerNavn = reader.GetString(8);

            LokaleData l = new LokaleData();
            l.LokaleNavn = reader.GetString(9);
            l.LokaleNummer = reader.GetString(10);
            l.LokaleSmartBoard = reader.GetBoolean(11);
            l.LokaleSize = (LokaleSize)reader.GetInt32(12);
            l.MuligeBookinger = reader.GetInt32(13);
            b.Lokale = l;
            


            //TODO tilføj brugerID og lokaleID foreign keys
            return b;
        }

        #endregion
        /// <summary>
        /// Oversætter data fra et Booking database kald til et BookingData object med alle columns.
        /// </summary>
        /// <param name="reader">Typen SqlDataReader. objekt med data fra database kald</param>
        /// <returns>et object af typen BookingData</returns>
        #region ReadBooking

        private BookingData ReadBookings(SqlDataReader reader)
        {
            BookingData k = new BookingData();

            LokaleData ld = new LokaleData();
            ld.LokaleNavn = reader.GetString(3);
            ld.LokaleNummer = reader.GetString(4);
            ld.LokaleSmartBoard = reader.GetBoolean(5);
            ld.LokaleSize = (LokaleSize) reader.GetInt32(6);
            ld.MuligeBookinger = reader.GetInt32(7);
            ld.LokaleID = reader.GetInt32(11);

            PersonData p = new PersonData();
            p.BrugerNavn = reader.GetString(8);

            k.Dag = reader.GetDateTime(i: 0);
            k.TidStart = reader.GetTimeSpan(i: 1);
            k.TidSlut = reader.GetTimeSpan(i: 2);
            k.Lokale = ld; //3,4,5,6,7
            k.Bruger = p; // 8
            k.ResevertionId = reader.GetInt32(9);
            k.HeltBooket = reader.GetInt32(10);

            return k;
        }

        #endregion
       
        

        
        /// <summary>
        /// Henter en booking baseret på id fra databasen
        /// </summary>
        /// <param name="id">Typen int. Indeholder id'et på den booking man vil hente fra databasen </param>
        /// <returns>Et objekt af typen BookingData</returns>
        public BookingData GetSingelBooking(int id)
        {
            BookingData l = new BookingData();
            string sql = "SELECT Reservation.Dag, Reservation.TidStart, Reservation.TidSlut, " +
                         "Lokale.LokaleNavn, LokaleLokation.LokaleNummer, LokaleSmartBoard, LokaleSize.Size, " +
                         "MuligeBookinger, Person.BrugerNavn, Reservation.ReservationID, Reservation.Heltbooket, Lokale.lokaleID " +
                         "FROM Reservation " +
                         "INNER JOIN Lokale ON Reservation.LokaleID_FK = Lokale.LokaleID " +
                         "inner join LokaleSize ON Lokale.LokaleSize_FK = SizeId " +
                         "inner join LokaleLokation ON Lokale.LokaleLokation_FK = LokaleLokationId " +
                         "INNER JOIN Person ON Reservation.BrugerID_FK = Person.BrugerID " +
                         "WHERE ReservationID = @id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    l = ReadBookings(reader);
                    return l;
                }

                return l;
            }
        }
        /// <summary>
        /// Opretter en ny booking og gemmer den i databasen
        /// </summary>
        /// <param name="newBooking">Typen BookingData. Indeholder den nye bookings information</param>
        public void AddReservation(BookingData newBooking)
        {
            string sql = "insert into Reservation VALUES (@tidStart, @dag, @mulige, @brugerFK, @lokaleFK, @tidSlut, @bookesFor)";
                                                        // TidStart, Dag, Heltbooket, Bruger_FK, Lokale_FK, TidSlut, BookesFor
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string tidSlut = newBooking.Dag.Add(newBooking.TidStart).ToShortTimeString();
                int brugerID = _logIndService.GetSingelPersonByEmail(newBooking.Bruger.BrugerEmail).BrugerID;

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@tidStart", newBooking.Dag.ToShortTimeString());
                cmd.Parameters.AddWithValue("@dag", newBooking.Dag.ToString("s"));
                cmd.Parameters.AddWithValue("@tidSlut", tidSlut);
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


        public void DeleteReservationById(int id)
        {
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


        /// <summary>
        /// Sletter en booking fra databasen dagen efter bookingen er overskredet
        /// </summary>
        public void DeleteReservationByDay()
        {
            string sql = "DELETE from Reservation WHERE Dag < @nextDay";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@nextDay", DateTime.Today.ToString("s"));

                cmd.Connection.Open();

                int rows = cmd.ExecuteNonQuery();
                //if (rows != 1)
                //{
                //    throw new InvalidOperationException("Der skete en fejl i databasen"); 
                //}
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
                    objects.Add(id);
                    objects.Add(limit);
                    objects.Add(senestBookingTid);
                }
            }

            return objects;
        }

    }
}