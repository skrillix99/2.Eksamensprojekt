using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuperBookerData;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace _2.Eksamensprojekt.Pages.LogInd
{
    public class LogIndModel : PageModel
    {
        public static LogIndData LoggedInUser { get; set; } = null;

        [BindProperty]
        public string EmailLogInd { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //TODO opret en error message hvis der er incorrect input i felterne ved hjælp af string message
        //public string Message { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            LogIndData logIndData = new LogIndData(EmailLogInd, Password);

            /*if (_brugerService.contains(logIndData))
            {
                LoggedInUser = logIndData;

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, EmailLogInd), 
                    new Claim(ClaimTypes.Role, brugerRolle.Administration.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));
                return RedirectToPage("/Persons/Index");
            }

            */
            return null;
        }
    }
}
