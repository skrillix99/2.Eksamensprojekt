using SuperBookerData;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using _2.Eksamensprojekt.Pages.LogInd;

namespace _2.Eksamensprojekt.Services
{
    public class LogIndService: ILogIndService
    {
        private const string connectionString = "Data Source=zealandmarc.database.windows.net;Initial Catalog=SuperBooker4000;User ID=AdminMarc;Password=Marcus19;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

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

        private PersonData ReadPersonID(SqlDataReader reader)
        {
            PersonData l = new PersonData()
            {
                BrugerID = reader.GetInt32(0)
            };

            return l;
        }

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

        public bool Contains(LogIndData LogInd)
        {
            string sql = "select BrugerEmail, BrugerPassword from Person where BrugerEmail = @email AND BrugerPassword = @password";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@email", LogInd.EmailLogInd);
                cmd.Parameters.AddWithValue("@password", LogInd.Password);
                cmd.Connection.Open();


            }

            return true;
        }

        public List<string> GetEmails()
        {
            List<string> list = new List<string>();
            string sql = "select BrugerEmail from Person";

            using (SqlConnection connection =  new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string s = ReadEmails(reader);
                    list.Add(s);
                }

                return list;
            }
        }

        private string ReadEmails(SqlDataReader reader)
        {
            string email = reader.GetString(0);
            return email;
        }

        public brugerRolle ContainsAndGiveRole(LogIndData LogInd)
        {
            if (!Contains(LogInd))
            {
                LogInd.rolle = brugerRolle.Student;
            }
            else
            {
                foreach (string email in GetEmails())
                {
                    LogInd.rolle = (LogInd.EmailLogInd.Equals(email)) ? brugerRolle.Underviser : brugerRolle.Administration;
                }
            }

            return LogInd.rolle;
        }

        public PersonData GetSingelPersonByEmail(string email)
        {
            PersonData list = new PersonData();

            string sql = "select BrugerID from Person WHERE BrugerEmail = @email";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var us = ReadPersonID(reader);
                    return us;
                }

                return list;
            }
        }
    }
}
