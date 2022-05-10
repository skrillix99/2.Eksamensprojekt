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
        [BindProperty]
        public LokaleData LokaleDataSingel { get; private set; }
        [BindProperty]
        public string NummerSøgning { get; set; }

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
            LokaleDataSingel = new LokaleData();
            string sql = "select * from Lokale where 1=1 ";

            if (!String.IsNullOrWhiteSpace(NummerSøgning))
            {
                sql += $"AND LokaleNummer = '{NummerSøgning}' ";
            }

            if (!(LokaleDataSingel.Etage <= 0) || (LokaleDataSingel.Etage >= 4))
            {
                sql += $"AND LokaleEtage = {LokaleDataSingel.Etage} ";
            }

            sql += $"SORT BY Etage";

            LokaleData = _ledigeLokalerService.GetAllLokaleBySqlString(sql);
        }


    }
}