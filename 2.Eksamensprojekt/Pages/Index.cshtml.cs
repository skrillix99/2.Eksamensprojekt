using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2.Eksamensprojekt.Pages.LogInd;
using _2.Eksamensprojekt.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace _2.Eksamensprojekt.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private IAdministrationService _administrationService;

        public IndexModel(ILogger<IndexModel> logger, IAdministrationService administrationService)
        {
            _logger = logger;
            _administrationService = administrationService;
        }

        public void OnGet()
        {
            _administrationService.DeleteResevation();

            if (LogIndModel.LoggedInUser == null)
            {
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            
        }

    }
}
