using SuperBookerData;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace _2.Eksamensprojekt.Services
{
    public class OpretBookingService : IOpretBookingService
    {
        private const string connectionString = @"Data Source=zealandmarc.database.windows.net;Initial Catalog=SuperBooker4000;User ID=AdminMarc;Password=**;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


        public BookingData create(BookingData newBooking)
        {
            String sql = "insert into Reservation(TidStart, Dag, HeltBooket, BrugerID_FK, LokaleID_FK, TidSlut) values(@TidStart, @Dag, @HeltBooket, @BrugerID_FK, @LokaleID_FK, @TidSlut)";

            // opret forbindelse
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                // åbner forbindelsen
                connection.Open();

                // opretter sql query
                SqlCommand cmd = new SqlCommand(sql, connection);

                // indsæt værdierne
                cmd.Parameters.AddWithValue("@TidStart", newBooking.TidStart);
                cmd.Parameters.AddWithValue("@Dag", newBooking.Dag);
                cmd.Parameters.AddWithValue("@HeltBooket", newBooking.HeltBooket);
                cmd.Parameters.AddWithValue("@BrugerID_FK", newBooking.BrugerID_FK);
                cmd.Parameters.AddWithValue("@LokaleID_FK", (int)newBooking.LokaleID_FK);
                cmd.Parameters.AddWithValue("@TidSlut", newBooking.TidSlut);

                // altid ved Insert, update, delete
                int rows = cmd.ExecuteNonQuery();

                if (rows != 1)
                {
                    // fejl
                    throw new ArgumentException("Not created");
                }

                return null;
            }
        }


    }
}
