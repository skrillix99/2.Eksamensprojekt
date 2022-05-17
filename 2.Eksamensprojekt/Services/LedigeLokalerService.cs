using SuperBookerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace _2.Eksamensprojekt.Services
{
    public class LedigeLokalerService : ILedigeLokalerService
    {
        private const string connectionString = @"Data Source=zealandmarc.database.windows.net;Initial Catalog=SuperBooker4000;User ID=AdminMarc;Password=Marcus19;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        /// <summary>
        /// Henter alle lokaler i databasen og lægger dem ind i et list objekt af typen LokaleData.
        /// </summary>
        /// <returns>Returnerer en liste af typen LokaleData</returns>
        public List<LokaleData> GetAll()
        {
            List<LokaleData> LkDataList = new List<LokaleData>();
            string sql = "SELECT * FROM Lokale " +
                         "inner join LokaleSize ON LokaleSize_FK = SizeId " +
                         "inner join LokaleLokation ON LokaleLokation_FK = LokaleLokationId";
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

        /// <summary>
        /// Oversætter data modtaget fra databasen og sætter dem ind i et LokaleData objekt
        /// </summary>
        /// <param name="reader">Typen SqlDataReader. Indeholder data modtaget fra databasen</param>
        /// <returns>Returnerer et objekt af typen LokaleData</returns>
        private LokaleData ReadLokaleData(SqlDataReader reader)
        {
            LokaleData ld1 = new LokaleData();

            ld1.LokaleID = reader.GetInt32(0);
            ld1.LokaleNavn = reader.GetString(1);
            ld1.LokaleSmartBoard = reader.GetBoolean(2);
            ld1.LokaleSize = (LokaleSize)reader.GetInt32(7);
            ld1.LokaleNummer = reader.GetString(10);
            ld1.MuligeBookinger = reader.GetInt32(8);
            ld1.Etage = reader.GetInt32(11);

            return ld1;
        }

        /// <summary>
        /// GetAllLokaleBySqlString funktionen er nødvendig for at sorteringen virker på "Ledige lokaler" siden.
        /// Den opretter først en tom liste, som skal blive brugt til at opbevarer informationen omkring de lokaler, som bliver beskrevet i SQL stringen. 
        /// Herefter opretter den forbindelse til vores database, bruger den sql kode som den har fra sin parameter, vælger alt data som passer til koden, 
        /// og putter dataen ind i den tomme liste. 
        /// </summary>
        /// <param name="sql">
        /// sql parameteren er den SQL kode som bliver brugt til at finde de lokaler som vi ønsker at finde, baseret på hvad vi har skrevet som vores sql string.
        /// </param>
        /// <returns>
        /// Returnerer alt information om alle lokaler, som passer til hvad SQL stringen beskriver.
        /// </returns>
        public List<LokaleData> GetAllLokaleBySqlString(string sql)
        {
            List<LokaleData> Lokaler = new List<LokaleData>();

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
                    Lokaler.Add(ld);
                }
            }
            return Lokaler;
        }
    }
    
}
