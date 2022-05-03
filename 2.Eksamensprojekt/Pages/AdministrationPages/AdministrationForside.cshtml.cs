using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _2.Eksamensprojekt.Pages.AdministrationPages
{
    [Authorize(Roles = "Administration")]
    public class AdministrationForsideModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
