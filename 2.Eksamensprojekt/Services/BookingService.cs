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
        private const string ConnectionString = @"Data Source=zealandmarc.database.windows.net;Initial Catalog=SuperBooker4000;User ID=AdminMarc;Password=Marcus19;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public List<BookingData> GetAllBookings()
        {
            List<BookingData> list = new List<BookingData>();

            string sql = "SELECT Reservation.Dag, Reservation.TidStart, Reservation.TidSlut, Lokale.LokaleNavn, LokaleLokation.LokaleNummer, " +
                         "LokaleSmartBoard, LokaleSize.Size, LokaleSize.MuligeBookinger, Person.BrugerNavn " +
                         "FROM Reservation " +
                         "INNER JOIN Lokale ON Reservation.ReservationID = Lokale.LokaleID " +
                         "INNER JOIN Person ON Reservation.ReservationID = Person.BrugerID " +
                         "inner join LokaleSize ON Lokale.LokaleSize_FK = SizeId inner join " +
                         "LokaleLokation ON Lokale.LokaleLokation_FK = LokaleLokationId";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    BookingData l = ReadBookings(reader);
                    list.Add(l);
                }

                return list;
            }
        }

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

            k.Dag = reader.GetDateTime(i: 0);
            k.TidStart = reader.GetTimeSpan(i: 1);
            k.TidSlut = reader.GetTimeSpan(i: 2);
            k.Lokale = ld; //3,4,5,6,7
            k.Bruger = p; // 8

            return k;
        }

        private LokaleData ReadLokale(SqlDataReader reader)
        {
            LokaleData k = new LokaleData();
            k.LokaleID = reader.GetInt32(0);
            k.LokaleNavn = reader.GetString(1);
            k.LokaleSmartBoard = reader.GetBoolean(2);
            k.LokaleSize = (LokaleSize)reader.GetInt32(7);
            k.LokaleNummer = reader.GetString(9);
            k.MuligeBookinger = reader.GetInt32(8);
            k.Etage = reader.GetInt32(10);

            return k;
        }


        public LokaleData GetById(int LokaleID)
        {
            String sql = "Select * from Lokale where LokaleID=@LokaleID";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand(sql, connection);

                cmd.Parameters.AddWithValue("@LokaleID", LokaleID);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    LokaleData k = ReadLokale(reader);
                    return k;
                }

            }

            return null;
        }


        public LokaleData Delete(int lokaleID)
        {
            LokaleData deletedk = GetById(lokaleID);

            String sql = "delete from LokalData where LokaleID=@LokalID";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand(sql, connection);

                cmd.Parameters.AddWithValue("@LokaleID", lokaleID);

                int rows = cmd.ExecuteNonQuery();

                if (rows != 1)
                {
                    //fejl
                }

                return deletedk;
            }
        }
    }
}