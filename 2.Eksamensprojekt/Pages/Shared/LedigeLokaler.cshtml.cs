using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2.Eksamensprojekt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Claims;
using SuperBookerData;

namespace _2.Eksamensprojekt.Pages.Shared
{
    public class LedigeLokalerModel : PageModel
    {
        private ILedigeLokalerService _ledigeLokalerService;
        private static List<LokaleData> _lokaleListe;
        public List<LokaleData> LokaleData { get; private set; }
        public LedigeLokalerModel(ILedigeLokalerService ledigeLokalerService)
        {
            _ledigeLokalerService = ledigeLokalerService;
        }
        
        public void OnGet()
        {
            _lokaleListe = _ledigeLokalerService.GetAll();
            LokaleData = new List<LokaleData>(_lokaleListe);
        }

        public void OnPost()
        {
            string sql = "select * from Lokale where 1=1 ";
            //if (LokaleNavn != null)
            //{
            //    sql = sql + "AND LokaleNummer = 1";
            //}
            //if (Etage != null)
            //{
            //    sql = sql + "AND Etage = 1";
            //}
            //if (datetime != null)
            //{
            //    sql = sql + "AND DateTime = 1";
            //}
            //if (LokaleSize != null)
            //{
            //    sql = sql + "AND Størrelse = 1"
            //}
            //if (LokaleSmartBoard != null)
            //{
            //    sql = sql + "AND LokaleSmartBoard = 1";
            //}

            sql = sql + "ORDER BY LokaleEtage";

        }


    }
}
