using System.Collections.Generic;
using System.Data.SqlClient;
using SuperBookerData;

namespace _2.Eksamensprojekt.Services.Interfaces
{
    public interface IAdministrationService
    {
        private const string connectionString = "Data Source=zealandmarc.database.windows.net;Initial Catalog=SuperBooker4000;User ID=AdminMarc;Password=********;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        List<LokaleData> GetAllLokaler()
        {
            List<LokaleData> lokaler = new List<LokaleData>();

            string sql = "select * from Lokale";

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
        }

        LokaleData ReadLokale(SqlDataReader reader);
    }
}