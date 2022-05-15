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

        /// <summary>
        /// GETALL metoden opretter en ny liste "LkDataList", som kommer til at blive brugt som opbevaring af informationen til ALLE lokaler.
        /// Herefter opretter den en sql string, som henter ALT information omkring de forskellige lokaler fra Lokale tabellen i vores database.
        /// Efter dette så opretter den forbindelse til vores database, og tilføjer informationen om de forskellige lokaler til LkDataList. 
        /// 
        /// Som den er lige nu returnerer den alle lokalerne, og ikke kun dem som er ledige.
        /// </summary>
        /// <returns>
        /// En liste over alle lokalerne i Lokale databasen.
        /// </returns>
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
        /// ReadLokaleData er den funktion som gør det muligt for de andre funktioner at læse dataen fra vores "Lokale" database.
        /// Årsagen til at readeren går fra 2 til 7, skyldes at den information der er på pladserne 3-6 er foreign keys,
        /// og har ingen relevans til den data vi ønsker at finde og vise. 
        /// </summary>
        /// <param name="reader">
        /// reader parameteren kommer fra SqlDataReader, som bliver sat på de andre funktioner. Den bruges til at indhente dataen fra vores database. 
        /// </param>
        /// <returns>
        /// Returnerer data omkring et lokale fra "lokale" databasen.
        /// </returns>
        private LokaleData ReadLokaleData(SqlDataReader reader)
        {
            LokaleData ld1 = new LokaleData();

            ld1.LokaleID = reader.GetInt32(0);
            ld1.LokaleNavn = reader.GetString(1);
            ld1.LokaleSmartBoard = reader.GetBoolean(2);
            ld1.LokaleSize = (LokaleSize)reader.GetInt32(7);
            ld1.LokaleNummer = reader.GetString(10);
            ld1.LokaleMuligeBookinger = reader.GetInt32(8);
            ld1.LokaleEtage = reader.GetInt32(11);

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
