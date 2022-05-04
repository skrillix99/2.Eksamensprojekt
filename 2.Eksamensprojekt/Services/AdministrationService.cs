using System.Collections.Generic;
using _2.Eksamensprojekt.Services.Interfaces;
using SuperBookerData;
using System.Data.SqlClient;

namespace _2.Eksamensprojekt.Services
{
    public class AdministrationService : IAdministrationService
    {
        private const string ConnectionString = "Data Source=zealandmarc.database.windows.net;Initial Catalog=SuperBooker4000;User ID=AdminMarc;Password=Marcus19;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        
        public List<LokaleData> GetAllLokaler()
        {
            List<LokaleData> lokaler = new List<LokaleData>();

            string sql = "select * from Lokale";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
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
        }

        public LokaleData GetSingelLokale(int id)
        {
            LokaleData l = new LokaleData();
            string sql = "select * from Lokale where LokaleID = @lokaleId";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@lokaleId", id);
                cmd.Connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    l = ReadLokale(reader);
                    return l;
                }
            }
            return l;
        }

        private LokaleData ReadLokale(SqlDataReader reader)
        {
            LokaleData l = new LokaleData();

            l.lokaleID = reader.GetInt32(0);
            l.lokaleNavn = reader.GetString(1);
            l.lokaleNummer = reader.GetString(2);
            l.lokaleSmartBoard = reader.GetBoolean(3);
            l.lokaleMuligeBookinger = reader.GetInt32(4);
            l.LokaleSize = (LokaleSize)reader.GetInt32(5);

            return l;
        }
    }
}