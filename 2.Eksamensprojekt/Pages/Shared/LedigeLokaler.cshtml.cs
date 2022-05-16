using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using _2.Eksamensprojekt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Claims;
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

        /// <summary>
        /// Bruges til at bestemme valgmulighederne i de forskellige s�gekriterier.
        /// </summary>
        public LedigeLokalerModel(ILedigeLokalerService ledigeLokalerService)
        {
            _ledigeLokalerService = ledigeLokalerService;
            SKEtage = new List<string>()
            {
                "V�lg etage", "Stue etage (D1)", "1. etage (D2)", "2. etage (D3)"
            };
            SKStoerrelse = new List<string>()
            {
               "V�lg lokale st�rrelse", "Gruppe lokaler", "Klasse lokaler", "Auditorie"
            };
            SKSmartBoard = new List<string>()
            {
                "Har SmartBoard?", "Ja", "Nej"
            };

        }

        /// <summary>
        /// N�r siden "Ledige lokaler" bliver loadet ind, s� henter den informationen omkring ALLE de lokaler der er i "Lokale" databasen og l�gger det ind i en liste
        /// s� det kan blive vist frem p� hjemmesiden.
        /// </summary>
        public void OnGet()
        {
            _lokaleListe = _ledigeLokalerService.GetAll();
            LokaleData = new List<LokaleData>(_lokaleListe);
        }

        /// <summary>
        /// OnPost metoden g�r at n�r der bliver trykket p� "S�g" knappen, opretter den en ny liste over lokalerne, som passer til SQL stringen. 
        /// Til at starte med er SQL stringen bare at den viser alle lokaler, men alt efter hvad brugeren har valgt, inden for de 3 s�gekriterier, s� tilpaser den
        /// SQL stringen, og g�r at lokalerne der passer til brugerens s�ge kriterier dukker op. 
        /// </summary>
        public void OnPost()
        {
            LokaleData = new List<LokaleData>();
            string sql = "SELECT * FROM Lokale inner join LokaleSize ON LokaleSize_FK = SizeId inner join LokaleLokation ON LokaleLokation_FK = LokaleLokationId WHERE 1=1";
            //  "V�lg Etage"
            if (SKEtage_valg == "Stue etage (D1)")
            {
                sql += "AND LokaleEtage = 1";
            }
            if (SKEtage_valg == "1. etage (D2)")
            {
                sql += "AND LokaleEtage = 2";
            }
            if (SKEtage_valg == "2. etage (D3)")
            {
                sql += "AND LokaleEtage = 3";
            }

            // "V�lg lokale st�rrelse"
            if (SKStoerrelse_valg == "Gruppe lokaler")
            {
                sql += "AND Size = 0";
            }
            if (SKStoerrelse_valg == "Klasse lokaler")
            {
                sql += "AND Size = 1";
            }
            if (SKStoerrelse_valg == "Auditorie")
            {
                sql += "AND Size = 2";
            }

            //  "Har SmartBoard?"
            if (SKSmartBoard_valg == "Ja")
            {
                sql += "AND LokaleSmartBoard = 1";
            }
            if (SKSmartBoard_valg == "Nej")
            {
                sql += "AND LokaleSmartBoard = 0 ";
            }


            LokaleData = _ledigeLokalerService.GetAllLokaleBySqlString(sql);


            //TODO S�RG FOR AT MAN KAN SORTERE EFTER DATO
            //   string sql = "select * from Lokale WHERE Dag between 'welp1' AND 'welp2'";
        }
    }
}