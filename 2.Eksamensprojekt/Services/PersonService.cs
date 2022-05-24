using System.Collections.Generic;
using System.Data.SqlClient;
using _2.Eksamensprojekt.Services.Interfaces;
using SuperBookerData;

namespace _2.Eksamensprojekt.Services
{
    public class PersonService: IPersonService // Marcus
    {
        private const string connectionString = "Data Source=zealandmarc.database.windows.net;Initial Catalog=SuperBooker4000;User ID=AdminMarc;Password=Marcus19;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        /// <summary>
        /// Henter alle Logind oplysninger i Databasen ned i en liste
        /// </summary>
        /// <returns>Returnerer en liste af typen LogIndData</returns>
        public List<LogIndData> GetPersoner()
        {
            List<LogIndData> list = new List<LogIndData>();
            string sql = "select BrugerEmail, BrugerPassword, BrugerRolle from Person";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    LogIndData l = ReadPersoner(reader);
                    list.Add(l);
                }

                return list;
            }

        }
        /// <summary>
        /// Oversætter data fra databasen og lægger det ind i et PersonData objekt
        /// </summary>
        /// <param name="reader">Typen SqlDataReader. Indeholder data fra Databasen</param>
        /// <returns>Returnerer et objekt af typen PersonData</returns>
        private PersonData ReadPersonID(SqlDataReader reader)
        {
            PersonData l = new PersonData()
            {
                BrugerID = reader.GetInt32(0)
            };

            return l;
        }
        /// <summary>
        /// Oversætter data fra databasen og lægger det ind i et PersonData objekt
        /// </summary>
        /// <param name="reader">Typen SqlDataReader. Indeholder data fra Databasen</param>
        /// <returns>Returnerer et objekt af typen PersonData</returns>
        private LogIndData ReadPersoner(SqlDataReader reader)
        {
            LogIndData l = new LogIndData
            {
                EmailLogInd = reader.GetString(0),
                Password = reader.GetString(1),
                rolle = (brugerRolle)reader.GetInt32(2)
            };

            return l;
        }
        
        /// <summary>
        /// Henter et person ID ud fra email fra databasen og lægger det ind i et objekt af typen PersonData
        /// </summary>
        /// <param name="email">Typen string. indeholder en email</param>
        /// <returns>Returnerer et objekt af typen PersonData</returns>
        public PersonData GetSingelPersonByEmail(string email)
        {
            PersonData pd = new PersonData();

            string sql = "select BrugerID from Person WHERE BrugerEmail = @email";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    pd = ReadPersonID(reader);
                }
            }
            return pd;
        }
    }
}