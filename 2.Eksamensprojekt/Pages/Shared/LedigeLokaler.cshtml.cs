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
        public List<string> SoegeKriterierVaerdier { get; private set; }
        
        [BindProperty]
        public string SoegeKriterier { get; set; }

        public LedigeLokalerModel(ILedigeLokalerService ledigeLokalerService)
        {
            _ledigeLokalerService = ledigeLokalerService;
            SoegeKriterierVaerdier = new List<string>()
            {
                "Lokale Nummer Stigende", "Lokale Nummer Faldende"
            };

        }
        public void OnGet()
        {
            _lokaleListe = _ledigeLokalerService.GetAll();
            LokaleData = new List<LokaleData>(_lokaleListe);

            
        }



        //public async Task<IActionResult> OnPost()
        //{
        //    if(user = brugerRolle.Student)
        //    {
        //        return RedirectToPage("StuderendePages/StuderendeBooking");
        //    }
        //    //DoFind();
        //    return Page();
        //}
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

        private void DoFind()
        {
            switch (SoegeKriterier)
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
                        //�bner forbindelsen
                        connection.Open();

                        //Opretter sql query
                        SqlCommand cmd = new SqlCommand(sql, connection);

                        //altid ved select
                        SqlDataReader reader = cmd.ExecuteReader();

                        //L�ser alle r�kker
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
