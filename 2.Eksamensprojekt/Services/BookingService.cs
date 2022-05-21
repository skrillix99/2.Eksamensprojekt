using SuperBookerData;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace _2.Eksamensprojekt.Services
{
    public class BookingService : IBookingService
    {

        /// <summary>
        /// opretter en variable ConnectionString som indeholder connection stringen for databasen.
        /// </summary>
        private const string ConnectionString = @"Data Source=zealandmarc.database.windows.net;Initial Catalog=SuperBooker4000;User ID=AdminMarc;Password=Marcus19;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        #region ReadBookings
        /// <summary>
        /// Oversætter data fra et Booking database kald til et BookingData object med alle columns.
        /// </summary>
        /// <param name="reader">Typen SqlDataReader. objekt med data fra database kald</param>
        /// <returns>et object af typen BookingData</returns>
        private BookingData ReadBookings(SqlDataReader reader)
        {
            BookingData k = new BookingData();

            LokaleData ld = new LokaleData();
            ld.LokaleNavn = reader.GetString(3);
            ld.LokaleNummer = reader.GetString(4);
            ld.LokaleSmartBoard = reader.GetBoolean(5);
            ld.LokaleSize = (LokaleSize)reader.GetInt32(6);
            ld.MuligeBookinger = reader.GetInt32(7);
            ld.LokaleID = reader.GetInt32(14);

            PersonData p = new PersonData();
            p.BrugerNavn = reader.GetString(8);
            p.brugerRolle = (brugerRolle)reader.GetInt32(10);
            p.BrugerEmail = reader.GetString(12);
            p.BrugerID = reader.GetInt32(13);

            k.Dag = reader.GetDateTime(i: 0);
            k.TidStart = reader.GetTimeSpan(i: 1);
            k.TidSlut = reader.GetTimeSpan(i: 2);
            k.Lokale = ld; //3,4,5,6,7
            k.Bruger = p; // 8
            k.ResevertionId = reader.GetInt32(9);
            k.BookesFor = (brugerRolle)reader.GetInt32(11);
            k.HeltBooket = reader.GetInt32(15);
            return k;
        }

        #endregion

        private BookingData ReadBookingsByRolle(SqlDataReader reader)
        {
            BookingData k = new BookingData();

            LokaleData ld = new LokaleData();
            ld.LokaleNavn = reader.GetString(9);
            ld.LokaleNummer = reader.GetString(10);
            ld.LokaleSmartBoard = reader.GetBoolean(11);
            ld.LokaleSize = (LokaleSize)reader.GetInt32(12);
            ld.MuligeBookinger = reader.GetInt32(13);            

            PersonData p = new PersonData();
            p.brugerRolle = (brugerRolle)reader.GetInt32(6);
            p.BrugerID = reader.GetInt32(7);
            p.BrugerNavn = reader.GetString(8);           

            k.ResevertionId = reader.GetInt32(i: 0);
            k.TidStart = reader.GetTimeSpan(i: 1);
            k.Dag = reader.GetDateTime(i: 2);
            k.HeltBooket = reader.GetInt32(3);
            k.TidSlut = reader.GetTimeSpan(4);
            k.BookesFor = (brugerRolle)reader.GetInt32(5);
            k.Lokale = ld; //3,4,5,6,7
            k.Bruger = p; // 8                       
            return k;
        }
        
        /// <summary>
        /// laver en liste og henter alle parameterne fra databasen og returner dem som en liste.
        /// </summary>
        /// <returns>en list med alle de valgte parameter fra databasen</returns>
        public List<BookingData> GetAllBookings()
        {
            
            // opretter en ny list af BookingData
            List<BookingData> list = new List<BookingData>();

            //fortæller hvad der skal hentes fra databasen i det her tilfælle fra flere tabler og den gør det ved hjælp af i inner join 
            String sql =
                "Select Dag, TidStart, TidSlut, LokaleNavn, LokaleNummer, LokaleSmartBoard, Size, Muligebookinger, " +
                "BrugerNavn, ReservationID, BrugerRolle, BookesFor, BrugerEmail, BrugerID, LokaleID From Reservation " +
                "INNER JOIN Person ON Reservation.BrugerID_FK = Person.BrugerID " +
                "INNER JOIN Lokale ON Reservation.LokaleID_FK = Lokale.LokaleID " +
                "INNER JOIN LokaleLokation ON Lokale.LokaleLokation_FK = LokaleLokation.LokaleLokationId " +
                "INNER JOIN LokaleSize ON Lokale.LokaleSize_FK = LokaleSize.SizeId ";
            
            //opretter forbindelsen
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                // //opretter sql query og åbner forbindelsen
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Connection.Open();

                //altid ved select
                SqlDataReader reader = cmd.ExecuteReader();

                //læser alle rækker
                while (reader.Read())
                {
                    BookingData l = ReadBookings(reader);
                    list.Add(l);
                }
                return list;
            }
        }

        /// <summary>
        /// Henter alle bookinger ned fra databasen baseret på brugerrolle
        /// </summary>
        /// <param name="sql2">Typen string. Indeholder WHERE argumentet til sql strengen</param>
        /// <returns>En liste af typen BookingData</returns>
        public List<BookingData> GetAllReservationerByRolle(string sql2)
        {
            List<BookingData> lokaler = new List<BookingData>();
            // dag, tidstart, tidslut, lokalenavn, lokalenummer, lokalesmartboard, size, muligebooker, brugernavn, reservationID, brugerrolle, 
            // bookesfor, brugeremail, 
            string sql = "Select ReservationID, TidStart, Dag, HeltBooket, TidSlut, BookesFor, BrugerRolle, BrugerID, BrugerNavn, " +
                         "LokaleNavn, LokaleNummer, LokaleSmartBoard, Size, Muligebookinger " +
                         "From Reservation " +
                         "INNER JOIN Person ON Reservation.BrugerID_FK = Person.BrugerID " +
                         "INNER JOIN Lokale ON Reservation.LokaleID_FK = Lokale.LokaleID " +
                         "INNER JOIN LokaleLokation ON Lokale.LokaleLokation_FK = LokaleLokation.LokaleLokationId " +
                         "INNER JOIN LokaleSize ON Lokale.LokaleSize_FK = LokaleSize.SizeId ";


            sql += sql2;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    BookingData l = ReadBookingsByRolle(reader);
                    lokaler.Add(l);
                }
            }

            return lokaler;
        }

        /// <summary>
        /// Henter en booking baseret på id fra databasen
        /// </summary>
        /// <param name="id">Typen int. Indeholder id'et på den booking man vil hente fra databasen </param>
        /// <returns>Et objekt af typen BookingData</returns>
        public BookingData GetSingleBooking(int id)
        { 
            BookingData l = new BookingData();
            string sql = "SELECT Dag, TidStart, TidSlut, LokaleNavn, LokaleNummer, LokaleSmartBoard, Size, Muligebookinger, " +
                         "BrugerNavn, ReservationID, BrugerRolle, BookesFor, BrugerEmail, BrugerID, Lokale.lokaleID, HeltBooket " +
                         "FROM Reservation " +
                         "INNER JOIN Lokale ON Reservation.LokaleID_FK = Lokale.LokaleID " +
                         "inner join LokaleSize ON Lokale.LokaleSize_FK = SizeId " +
                         "inner join LokaleLokation ON Lokale.LokaleLokation_FK = LokaleLokationId " +
                         "INNER JOIN Person ON Reservation.BrugerID_FK = Person.BrugerID " +
                         "WHERE ReservationID = @id";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
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
        /// sletter en booking fra databasen ud fra fundet id og smider en exception ugyldigt id hvis id ikke er fundet
        /// </summary>
        /// <param name="id"></param>
        public void DeleteReservationById(int id)
        {
            if (id <= 0)
            {
                throw new KeyNotFoundException("Ugyldigt ID");
            }
            string sql = "DELETE from Reservation WHERE ReservationID = @id";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
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

            using (SqlConnection connection = new SqlConnection(ConnectionString))
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


    }
}