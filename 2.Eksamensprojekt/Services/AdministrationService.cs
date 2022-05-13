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
        private ILogIndService _logIndService;
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

        private BookingData ReadReservation(SqlDataReader reader)
        {
            BookingData b = new BookingData();
            b.TidStart = reader.GetTimeSpan(0);
            b.Dag = reader.GetDateTime(1);
            b.HeltBooket = reader.GetInt32(2);
            b.TidSlut = reader.GetTimeSpan(3); 
            //TODO tilføj brugerID og lokaleID foreign keys
            return b;
        }

        #endregion

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

            PersonData p = new PersonData();
            p.BrugerNavn = reader.GetString(8);

            k.Dag = reader.GetDateTime(i: 0);
            k.TidStart = reader.GetTimeSpan(i: 1);
            k.TidSlut = reader.GetTimeSpan(i: 2);
            k.Lokale = ld; //3,4,5,6,7
            k.Bruger = p; // 8
            k.ResevertionId = reader.GetInt32(9);

            return k;
        }

        #endregion

        public List<LokaleData> GetAllLokaler()
        {
            List<LokaleData> lokaler = new List<LokaleData>();

            string sql = "select * from Lokale " +
                         "inner join LokaleSize ON LokaleSize_FK = SizeId " +
                         "inner join LokaleLokation ON LokaleLokation_FK = LokaleLokationId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    LokaleData l = ReadLokale(reader);
                    lokaler.Add(l);
                }
            }

            return lokaler;
        } //TODO need?

        public LokaleData GetSingelLokale(int id)
        {
            LokaleData list = new LokaleData();

            string sql = "select * from Lokale  " +
                         "inner join LokaleSize ON LokaleSize_FK = SizeId " +
                         "inner join LokaleLokation ON LokaleLokation_FK = LokaleLokationId " +
                         "WHERE LokaleID = @id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var us = ReadLokale(reader);
                    return us;
                }

                return list;
            }
        }

        public List<BookingData> GetAllReservationer()
        {
            List<BookingData> lokaler = new List<BookingData>();

            string sql = "select * from Reservation";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    BookingData l = ReadReservation(reader);
                    lokaler.Add(l);
                }
            }

            return lokaler;
        } //TODO need?

        public BookingData GetSingelBooking(int id)
        {
            BookingData l = new BookingData();
            string sql = "SELECT Reservation.Dag, Reservation.TidStart, Reservation.TidSlut, " +
                         "Lokale.LokaleNavn, LokaleLokation.LokaleNummer, LokaleSmartBoard, LokaleSize.Size, MuligeBookinger, Person.BrugerNavn, ReservationID " +
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

        //public void AddReservation(BookingData newBooking)
        //{
        //    string sql = "insert into Reservation VALUES (@tidStart, @dag, 0, @brugerFK, @lokaleFK, @tidSlut, @bookesFor)";
        //                                                // TidStart, Dag, Heltbooket, Bruger_FK, Lokale_FK, TidSlut, BookesFor
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        TimeSpan tidSlut= newBooking.Dag.TimeOfDay.Add(newBooking.TidStart);
        //        int brugerID = _logIndService.GetSingelPersonByEmail(newBooking.Bruger.BrugerEmail).BrugerID;

        //        SqlCommand cmd = new SqlCommand(sql, connection);
        //        cmd.Parameters.AddWithValue("@tidStart", newBooking.TidStart.ToString());
        //        cmd.Parameters.AddWithValue("@dag", newBooking.Dag.ToString("s"));
        //        cmd.Parameters.AddWithValue("@tidSlut", tidSlut.ToString());
        //        cmd.Parameters.AddWithValue("@brugerFK", brugerID);
        //        cmd.Parameters.AddWithValue("@lokaleFK", newBooking.Lokale.LokaleID);
        //        cmd.Parameters.AddWithValue("@bookesFor", (int)newBooking.BookesFor);

        //        cmd.Connection.Open();

        //        int rows = cmd.ExecuteNonQuery();

        //        if (rows != 1)
        //        {
        //            throw new Exception("welp");
        //        }
        //    }

        //}

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

                cmd.Connection.Open();

                int rows = cmd.ExecuteNonQuery();

                if (rows != 1)
                {
                    throw new Exception("welp");
                }
            }

        }

        public void DeleteReservation(int id, DateTime dag)
        {
            DateTime dt = DateTime.Now.AddDays(-3); //TODO logic ændres
            int newDay = dag.Subtract(dt).Days;
            if (!(dag.Subtract(dt).Days >= 3))
            {
                throw new ArgumentOutOfRangeException("Må kun annulere med minimum 3 dages varsel.");
            }

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


        public void DeleteReservation()
        {
            //if (id <= 0)
            //{
            //    throw new KeyNotFoundException("Der findes ikke nogle reservationer med det ID");
            //}

            //string sql = "DELETE from Reservation WHERE ReservationID = @id";
            string sql = "DELETE from Reservation WHERE Dag < @nextDay";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
                //cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@nextDay", DateTime.Today.ToString("s"));

                cmd.Connection.Open();

                int rows = cmd.ExecuteNonQuery();
                //if (rows != 1)
                //{
                //    throw new InvalidOperationException("Der skete en fejl i databasen"); 
                //}
            }
        }

    }
}