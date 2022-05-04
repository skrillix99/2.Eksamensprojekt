using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuperBookerData;

namespace _2.Eksamensprojekt.Pages.Shared
{
    public class LedigeLokalerModel : PageModel
    {
        private static List<LokaleData> _lokaleListe;

        public List<LokaleData> LokaleData { get; private set; } 
        public void OnGet()
        {

        }
    }
}
