using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using _2.Eksamensprojekt.Pages.AdministrationPages;
using _2.Eksamensprojekt.Services.Interfaces;
using SuperBookerData;

namespace _2.Eksamensprojekt.Services // Marcus
{
    public class StuderendeService: IStuderendeService
    {
        private const string connectionString = "Data Source=zealandmarc.database.windows.net;Initial Catalog=SuperBooker4000;User ID=AdminMarc;Password=Marcus19;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private readonly IPersonService _personService;
        private readonly ILokalerService _lokalerService;
        private readonly IAdministrationService _administrationService;
        private readonly IBookingService _bookingService;
        /// <summary>
        /// Laver dependency injection til at kunne bruge services.
        /// </summary>
        /// <param name="personService">Typen IPersonService</param>
        public StuderendeService(IPersonService personService, ILokalerService lokalerService, IAdministrationService administrationService, IBookingService bookingService)
        {
            _personService= personService;
            _lokalerService = lokalerService;
            _administrationService = administrationService;
            _bookingService = bookingService;
        }

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
            ld.LokaleID = reader.GetInt32(12);

            PersonData p = new PersonData();
            p.BrugerNavn = reader.GetString(8);
            p.BrugerID = reader.GetInt32(10);

            k.Dag = reader.GetDateTime(0);
            k.TidStart = reader.GetTimeSpan(1);
            k.TidSlut = reader.GetTimeSpan(2);
            k.Lokale = ld; //3,4,5,6,7
            k.Bruger = p; // 8
            k.ResevertionId = reader.GetInt32(9);
            k.HeltBooket = reader.GetInt32(11);

            return k;
        }

        #endregion


        /// <summary>
        /// Reservere et lokale for en studerende baseret på deres BrugerID og BookingDataen der bliver sendt med i parameteret.
        /// Tjekker om brugeren har for mange bookinger.
        /// Finder ud af om der er nogle som har booket lokale på den samme dag og om lokalet er heltbooket eller ej.
        /// Hvis lokalet ikke er blevet booket før så kalder den GetSingelLokale() og tager MuligeBookinger og minusser den med 1, som den giver HeltBooket.
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

            int BrugerID = _personService.GetSingelPersonByEmail(newBooking.BrugerEmail).BrugerID;
            int limit = (int) _administrationService.GetAllStuderendeRettigheder()[1];
            int antalBooket = CheckReservationerByBrugerId(BrugerID).Count;
            //tjekker om brugeren har for mange bookinger
            if (antalBooket <= limit)
            {
                msg = $"Du har nu {limit - CheckReservationerByBrugerId(BrugerID).Count} tilbage";
            }
            else
            {
                throw new ArgumentOutOfRangeException($"Du må kun have {limit} bookinger ad gangen");
            }
            //failsafe. hvis lokaleID er 0 så finder vi den rigtige.
            if (newBooking.Lokale.LokaleID == 0)
            {
                newBooking.Lokale.LokaleID = _bookingService.GetSingleBooking(newBooking.ResevertionId).Lokale.LokaleID;
            }

            //findbinder til databasen via connectionString i toppen af klassen
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                BookingData tilbage = GetSingleBookingByIdAndDay(newBooking.Lokale.LokaleID, newBooking.Dag);
                //hvis der ikke er nogle bookinger i det lokale på den dag så finder den ud af
                //hvor mange der kan booke lokalet fra starten og minusser det med 1 senere henne
                if (tilbage == null)
                {
                    tilbage = new BookingData();
                    tilbage.HeltBooket = _lokalerService.GetSingelLokale(newBooking.Lokale.LokaleID).MuligeBookinger;
                }
                // hvis lokalet er booket på den dag så tjekker vi om heltbooket er 0
                else if (tilbage.HeltBooket == 0)
                {
                    throw new ArgumentOutOfRangeException("Der er ikke flere bookings ledige på dette lokale");
                }

                // laver vores SQL sætninger klar til SqlCommand
                string sql = "insert into Reservation VALUES (@tidStart, @dag, @mulige, @brugerFK, @lokaleFK, @tidSlut, 0, @smartboard)";
                string sqlUpdate = "UPDATE Reservation SET HeltBooket = @heltBooket WHERE LokaleID_FK = @id AND Dag = @dag";

                // kører sql sætningen på databasen med de parameter vi sender med.
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@tidStart", newBooking.TidStart);
                cmd.Parameters.AddWithValue("@dag", newBooking.Dag.Date);
                cmd.Parameters.AddWithValue("@mulige", tilbage.HeltBooket);
                cmd.Parameters.AddWithValue("@tidSlut", newBooking.TidSlut);
                cmd.Parameters.AddWithValue("@brugerFK", BrugerID);
                cmd.Parameters.AddWithValue("@lokaleFK", newBooking.Lokale.LokaleID);
                cmd.Parameters.AddWithValue("@smartboard", newBooking.BooketSmartBoard);

                //åbner for forbindelsen til databasen.
                cmd.Connection.Open();

                int rows = cmd.ExecuteNonQuery();

                if (rows != 1)
                {
                    throw new Exception("Der skete en fejl med databasen. prøv senere");
                }
                // lukker for forbindelsen.
                cmd.Connection.Close();

                // updatere Heltbooket med - 1 baseret på lokaleId og Dag
                SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, connection);
                cmdUpdate.Parameters.AddWithValue("@heltBooket", tilbage.HeltBooket - 1);
                cmdUpdate.Parameters.AddWithValue("@id", newBooking.Lokale.LokaleID);
                cmdUpdate.Parameters.AddWithValue("@dag", newBooking.Dag);

                cmdUpdate.Connection.Open();

                int rowsUpdate = cmdUpdate.ExecuteNonQuery();
                if (rowsUpdate < 1)
                {
                    throw new Exception("Der skete en fejl med databasen. prøv senere");
                }
            }

            return msg;
        }

        /// <summary>
        /// Sletter en reservation baseret på id'et fra parameteret
        /// </summary>
        /// <param name="id">typen Int. Skal være et gyldigt reservations id</param>
        public void DeleteReservation(int id)
        {
            string sql = "DELETE from Reservation WHERE ReservationID = @id";
            string sqlUpdate = "UPDATE Reservation SET HeltBooket = @heltBooket WHERE LokaleID_FK = @id AND dag = @dag";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                BookingData bd = _bookingService.GetSingleBooking(id);

                SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, connection);
                cmdUpdate.Parameters.AddWithValue("@heltBooket", bd.HeltBooket + 1);
                cmdUpdate.Parameters.AddWithValue("@id", bd.Lokale.LokaleID);
                cmdUpdate.Parameters.AddWithValue("@dag", bd.Dag);

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

        /// <summary>
        /// Finder ReservationID på alle de bookinger en bruger har via parameter id'et
        /// </summary>
        /// <param name="id">Typen Int. Skal være et gyldigt brugerID</param>
        /// <returns>Liste object af BookingData</returns>
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
                    BookingData bd = new();
                    bd.ResevertionId = reader.GetInt32(0);
                    lokaler.Add(bd);
                }
            }

            return lokaler;
        }
        /// <summary>
        /// Finder ud af om lokalet er booket på den dag via parameterne.
        /// </summary>
        /// <param name="id">Typen Int. Skal være et LokaleID</param>
        /// <param name="dag">Typen DateTime. Skal være dagen man vil reservere lokalet på</param>
        /// <returns>object af typen BookingData</returns>
        public BookingData GetSingleBookingByIdAndDay(int id, DateTime dag)
        {
            BookingData l = new BookingData();
            string sql = "SELECT Reservation.Dag, Reservation.TidStart, Reservation.TidSlut, " +
                         "Lokale.LokaleNavn, LokaleLokation.LokaleNummer, LokaleSmartBoard, LokaleSize.Size, " +
                         "MuligeBookinger, Person.BrugerNavn, Reservation.ReservationID, BrugerID, Reservation.Heltbooket, Lokale.lokaleID " +
                         "FROM Reservation " +
                         "INNER JOIN Lokale ON Reservation.LokaleID_FK = Lokale.LokaleID " +
                         "inner join LokaleSize ON Lokale.LokaleSize_FK = SizeId " +
                         "inner join LokaleLokation ON Lokale.LokaleLokation_FK = LokaleLokationId " +
                         "INNER JOIN Person ON Reservation.BrugerID_FK = Person.BrugerID " +
                         "WHERE LokaleID = @id AND Reservation.Dag = @dag";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@dag", dag.Date.ToString("yyyy-MM-ddTHH:mm:ss"));
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