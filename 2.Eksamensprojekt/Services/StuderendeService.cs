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
        private readonly ILogIndService _logIndService;
        private readonly IAdministrationService _administrationService;
        /// <summary>
        /// Laver dependency injection til at kunne bruge ILogIndService.
        /// </summary>
        /// <param name="logIndService">Typen ILogIndService</param>
        public StuderendeService(ILogIndService logIndService, IAdministrationService administrationService)
        {
            _logIndService = logIndService;
            _administrationService = administrationService;
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

        #region ReadBooking

        private BookingData ReadBookings(SqlDataReader reader)
        {
            BookingData k = new BookingData();

            LokaleData ld = new LokaleData();
            ld.LokaleNavn = reader.GetString(3);
            ld.LokaleNummer = reader.GetString(4);
            ld.LokaleSmartBoard = reader.GetBoolean(5);
            ld.LokaleSize = (LokaleSize)reader.GetInt32(6);
            ld.MuligeBookinger = reader.GetInt32(7);

            PersonData p = new PersonData();
            p.BrugerNavn = reader.GetString(8);
            p.BrugerID = reader.GetInt32(10);

            k.Dag = reader.GetDateTime(i: 0);
            k.TidStart = reader.GetTimeSpan(i: 1);
            k.TidSlut = reader.GetTimeSpan(i: 2);
            k.Lokale = ld; //3,4,5,6,7
            k.Bruger = p; // 8
            k.ResevertionId = reader.GetInt32(9);
            k.HeltBooket = reader.GetInt32(11);

            return k;
        }

        #endregion


        /// <summary>
        /// Reservere et lokale for en studerende baseret på deres BrugerID. Hvis lokalet ikke er blevet booket før
        /// så kalder den GetSingelLokale() og tager MuligeBookinger og minusser den med 1, som den giver HeltBooket.
        /// Hvis HeltBooket = 0 så thrower den en ArgumentOutOfRangeException.
        /// hvis kaldet til databasen fejler så thrower den en Execption
        /// </summary>
        /// <param name="newBooking">Typen BookingData.</param>
        /// <returns>string med en besked om hvor mange reservationer man har tilbage</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="Exception"></exception>
        public string AddReservation(BookingData newBooking)
        {
            string msg;

            int BrugerID = _logIndService.GetSingelPersonByEmail(newBooking.BrugerEmail).BrugerID;
            if (CheckReservationerByBrugerId(BrugerID).Count <= 3)
            {
                msg = $"Du har nu {3 - CheckReservationerByBrugerId(BrugerID).Count} tilbage";
            }
            else
            {
                throw new ArgumentOutOfRangeException("Du må kun have 3 bookings ad gangen");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                BookingData tilbage = GetSingelBooking(newBooking.Lokale.LokaleID);
                if (tilbage == null)
                {
                    tilbage = new BookingData();
                    tilbage.HeltBooket = GetSingelLokale(newBooking.Lokale.LokaleID).MuligeBookinger;
                }
                else if (tilbage.HeltBooket == 0)
                {
                    throw new ArgumentOutOfRangeException("Der er ikke flere bookings ledige på dette lokale");
                }


                string tidSlut = newBooking.Dag.Add(newBooking.TidStart).ToShortTimeString();

                string sql = "insert into Reservation VALUES (@tidStart, @dag, @mulige, @brugerFK, @lokaleFK, @tidSlut, 0)";
                string sqlUpdate = "UPDATE Reservation SET HeltBooket = @heltBooket WHERE LokaleID_FK = @id";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@tidStart", newBooking.Dag.ToShortTimeString());
                cmd.Parameters.AddWithValue("@dag", newBooking.Dag.ToString("s"));
                cmd.Parameters.AddWithValue("@mulige", tilbage.HeltBooket);
                cmd.Parameters.AddWithValue("@tidSlut", tidSlut);
                cmd.Parameters.AddWithValue("@brugerFK", BrugerID);
                cmd.Parameters.AddWithValue("@lokaleFK", newBooking.Lokale.LokaleID);

                cmd.Connection.Open();

                int rows = cmd.ExecuteNonQuery();

                if (rows != 1)
                {
                    throw new Exception("welp");
                }
                cmd.Connection.Close();


                SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, connection);
                cmdUpdate.Parameters.AddWithValue("@heltBooket", tilbage.HeltBooket - 1);
                cmdUpdate.Parameters.AddWithValue("@id", newBooking.Lokale.LokaleID);

                cmdUpdate.Connection.Open();

                int rowsUpdate = cmdUpdate.ExecuteNonQuery();
                if (rowsUpdate < 1)
                {
                    throw new Exception("humu humu");
                }
            }

            return msg;
        }

        public LokaleData GetSingelLokale(int id)
        {
            LokaleData list = new LokaleData();

            string sql = "select * from Lokale " +
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

        public void DeleteReservation(int id)
        {
            string sql = "DELETE from Reservation WHERE ReservationID = @id";
            string sqlUpdate = "UPDATE Reservation SET HeltBooket = @heltBooket WHERE LokaleID_FK = @id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                BookingData bd = _administrationService.GetSingelBooking(id);

                SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, connection);
                cmdUpdate.Parameters.AddWithValue("@heltBooket", bd.HeltBooket + 1);
                cmdUpdate.Parameters.AddWithValue("@id", bd.Lokale.LokaleID);

                cmdUpdate.Connection.Open();

                int rowsUpdate = cmdUpdate.ExecuteNonQuery();
                if (rowsUpdate < 1)
                {
                    throw new InvalidOperationException("Der skete en fejl i databasen. update");
                }
                cmdUpdate.Connection.Close();


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

        public List<BookingData> GetAllReservationer(string sql2)
        {
            throw new NotImplementedException();
        }

        public List<BookingData> CheckReservationerByBrugerId(int id)
        {
            List<BookingData> lokaler = new List<BookingData>();

            string sql = "Select ReservationID " +
                         "From Reservation INNER JOIN Person ON Reservation.BrugerID_FK = Person.BrugerID " +
                         "INNER JOIN Lokale ON Reservation.LokaleID_FK = Lokale.LokaleID " +
                         "INNER JOIN LokaleLokation ON Lokale.LokaleLokation_FK = LokaleLokation.LokaleLokationId " +
                         "INNER JOIN LokaleSize ON Lokale.LokaleSize_FK = LokaleSize.SizeId " +
                         "WHERE Person.BrugerID = @id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Connection.Open();
                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    BookingData l = ReadReservationByBrugerId(reader);
                    lokaler.Add(l);
                }
            }

            return lokaler;
        }

        private BookingData ReadReservationByBrugerId(SqlDataReader reader)
        {
            BookingData bd = new BookingData();

            bd.ResevertionId = reader.GetInt32(0);

            return bd;
        }

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

                if (!reader.HasRows)
                {
                    return null;
                }
                return l;
            }
        }
    }
}