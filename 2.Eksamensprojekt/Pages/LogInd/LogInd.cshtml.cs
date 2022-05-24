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
using _2.Eksamensprojekt.Services;
using _2.Eksamensprojekt.Services.Interfaces;

namespace _2.Eksamensprojekt.Pages.LogInd
{
    public class LogIndModel : PageModel
    {
        private IPersonService _brugerService;
        public static LogIndData LoggedInUser { get; set; } = null;

        public string Errormsg { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Du skal udfylde feltet")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Ugyldig Zealand E-mail")]
        [MinLength(12, ErrorMessage = "Email kan ikke være kortere end 12 tegn")]
        public string EmailLogInd { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Du skal udfylde adgangskode feltet.")]
        //TODO make a regularexpression
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public LogIndModel(IPersonService brugerService)
        {
            _brugerService = brugerService;
        }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost()
        {
            List<LogIndData> logIndData = _brugerService.GetPersoner();
            foreach (LogIndData user in logIndData)
            {

                if (EmailLogInd.ToLower() == user.EmailLogInd.ToLower() && Password == user.Password)
                {
                    LoggedInUser = user;
                    // sætter Claims op med Email (ClaimTypes.Name) og Rolle (ClaimTypes.Role) og bagefter redirect'er til den rette forside baseret på role.
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, EmailLogInd), 
                    new Claim(ClaimTypes.Role, user.rolle.ToString())
                };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    if (claims[1].Value == brugerRolle.Student.ToString()) //TODO cleanup?
                    {
                        return RedirectToPage("/Shared/LedigeLokaler");
                    }
                    if (claims[1].Value == brugerRolle.Underviser.ToString())
                    {
                        return RedirectToPage("/Shared/LedigeLokaler");
                    }
                    if (claims[1].Value == brugerRolle.Administration.ToString())
                    {
                        return RedirectToPage("/Shared/LedigeLokaler");
                    }
                }
            }

            return Page();
        }
    }
}
