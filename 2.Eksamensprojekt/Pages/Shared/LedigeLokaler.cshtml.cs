using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using _2.Eksamensprojekt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuperBookerData;

namespace _2.Eksamensprojekt.Pages.Shared
{
    public class LedigeLokalerModel : PageModel
    {
        private ILedigeLokalerService _ledigeLokalerService;
        private static List<LokaleData> _lokaleListe;
        

        public List<LokaleData> LokaleData { get; private set; }
        public List<string> SortTypeValues { get; private set; }
        
        [BindProperty]
        public string SortType { get; set; }

        public LedigeLokalerModel(ILedigeLokalerService ledigeLokalerService)
        {
            _ledigeLokalerService = ledigeLokalerService;
            SortTypeValues = new List<string>()
            {
                "Lokale Nummer Stigende", "Lokale Nummer Faldende"
            };

        }
        public void OnGet()
        {
            _lokaleListe = _ledigeLokalerService.GetAll();
            LokaleData = new List<LokaleData>(_lokaleListe);
        }

        public IActionResult OnPost()
        {
            DoFind();
            return Page();
        }
        private LokaleData ReadLokaleData(SqlDataReader reader)
        {
            LokaleData ld1 = new LokaleData();

            ld1.lokaleID = reader.GetInt32(0);
            ld1.lokaleNavn = reader.GetString(1);
            ld1.lokaleNummer = reader.GetString(2);
            ld1.lokaleSmartBoard = reader.GetBoolean(3);
            ld1.LokaleSize = (LokaleSize)reader.GetInt32(4);
            ld1.lokaleMuligeBookinger = reader.GetInt32(5);

            return ld1;
        }

        private void DoFind()
        {
            switch (SortType)
            {
                case "Lokale Nummer Stigende":
                    LokaleData = new List<LokaleData>(_lokaleListe);
                    break;

                case "Lokale Nummer Faldende":
                    List<LokaleData> LkDataList = new List<LokaleData>();
                    string sql = "SELECT * FROM Lokale ORDER BY LokaleNummer";
                    const string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SuperBookerLokal;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
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
                    break;
            }
        }
    }
}
