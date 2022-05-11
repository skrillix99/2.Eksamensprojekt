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

        //MARCUS KODE DO NOT DELETE XD
        //public async Task OnGetAsync()
        //{
        //    // live update search stuff (forhåblig) ToListAsync??? problem
        //    var movies = from m in _ledigeLokalerService.GetAll()
        //                 select m;
        //    if (!string.IsNullOrEmpty(SoegeKriterier))
        //    {
        //        movies = movies.Where(s => s.LokaleNummer.Contains(SoegeKriterier));
        //    }

        //    LokaleData = await movies.ToListAsync();
        //}

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
            ld1.LokaleSmartBoard = reader.GetBoolean(2);
            ld1.LokaleSize = (LokaleSize)reader.GetInt32(7);
            ld1.LokaleNummer = reader.GetString(9);
            ld1.LokaleMuligeBookinger = reader.GetInt32(8);
            ld1.LokaleEtage = reader.GetInt32(10);

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
