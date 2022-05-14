using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using _2.Eksamensprojekt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuperBookerData;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;


namespace _2.Eksamensprojekt.Pages.Shared
{
    public class LedigeLokalerModel : PageModel
    {
        private ILedigeLokalerService _ledigeLokalerService;
        private static List<LokaleData> _lokaleListe;
        
        

        public List<LokaleData> LokaleData { get; private set; }
        public List<string> SKEtage { get; private set; }
        public List<string> SKStoerrelse { get; private set; }
        public List<string> SKSmartBoard { get; private set; }

        [BindProperty]
        public string SKEtage_valg { get; set; }

        [BindProperty]
        public string SKStoerrelse_valg { get; set; }

        [BindProperty]
        public string SKSmartBoard_valg { get; set; }

        public LedigeLokalerModel(ILedigeLokalerService ledigeLokalerService)
        {
            _ledigeLokalerService = ledigeLokalerService;
            SKEtage = new List<string>()
            {
                "Vælg etage", "Alle etager", "Stue etage (D1)", "1. etage (D2)", "2. etage (D3)"
            };
            SKStoerrelse = new List<string>()
            {
               "Vælg lokale størrelse", "Alle lokaler", "Gruppe lokaler", "Klasse lokaler", "Auditorie"
            };
            SKSmartBoard = new List<string>()
            {
                "Har SmartBoard?", "Ja", "Nej"
            };

        }

        public void OnGet()
        {
            _lokaleListe = _ledigeLokalerService.GetAll();
            LokaleData = new List<LokaleData>(_lokaleListe);
        }

        public void OnPost()
        {
              string sql = $"SELECT * from UserStory WHERE Id = {id}";

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
                    UserStory us = ReadUserStory(reader);
                    return us;
                }

            }
            return null;
         //   string sql = "select * from Lokale WHERE Dag between 'welp1' AND 'welp2'";
        }

        private LokaleData ReadLokaleData(SqlDataReader reader)
        {
            LokaleData ld1 = new LokaleData();

            ld1.LokaleID = reader.GetInt32(0);
            ld1.LokaleNavn = reader.GetString(1);
            ld1.LokaleSmartBoard = reader.GetBoolean(2);
            ld1.LokaleSize = (LokaleSize)reader.GetInt32(7);
            ld1.LokaleNummer = reader.GetString(9);
            ld1.LokaleMuligeBookinger = reader.GetInt32(8);
            ld1.LokaleEtage = reader.GetInt32(10);

            return ld1;
        }
    }
}
