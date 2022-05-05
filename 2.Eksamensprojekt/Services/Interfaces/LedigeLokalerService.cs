using SuperBookerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuperBookerData;
using System.Data.SqlClient;

namespace _2.Eksamensprojekt.Services
{
    public class LedigeLokalerService : ILedigeLokalerService
    {
        private const string connectionString = @"Data Source=zealandmarc.database.windows.net;Initial Catalog=SuperBooker4000;User ID=AdminMarc;Password=Marcus19;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public List<LokaleData> GetAll()
        {
            List<LokaleData> LkDataList = new List<LokaleData>();
            string sql = "SELECT * FROM Lokale";
            //Opretter forbindelse
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //åbner forbindelsen
                connection.Open();

                //Opretter sql query
                SqlCommand cmd = new SqlCommand(sql, connection);

                //altid ved select
                SqlDataReader reader = cmd.ExecuteReader();

                //Læser alle rækker
                while (reader.Read())
                {
                    LokaleData ld = ReadLokaleData(reader);
                    LkDataList.Add(ld);
                }
            }
            return LkDataList;
        }
        private LokaleData ReadLokaleData(SqlDataReader reader)
        {
            LokaleData ld1 = new LokaleData();

            ld1.LokaleID = reader.GetInt32(0);
            ld1.LokaleNavn = reader.GetString(1);
            ld1.LokaleNummer = reader.GetString(2);
            ld1.LokaleSmartBoard = reader.GetBoolean(3);
            ld1.LokaleSize = (LokaleSize)reader.GetInt32(4);
            ld1.LokaleMuligeBookinger = reader.GetInt32(5);

            return ld1;
        }
    }

}