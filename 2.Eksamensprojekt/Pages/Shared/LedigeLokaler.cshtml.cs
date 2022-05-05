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
using Claim = Microsoft.IdentityModel.Claims.Claim;
using SuperBookerData;

namespace _2.Eksamensprojekt.Pages.Shared
{
    public class LedigeLokalerModel : PageModel
    {
        private ILedigeLokalerService _ledigeLokalerService;
        private static List<LokaleData> _lokaleListe;
        public List<LokaleData> LokaleData { get; private set; }

        public string TestA { get; set; }
        public string TestB { get; set; }
        public string TestC { get; set; }
        public LedigeLokalerModel(ILedigeLokalerService ledigeLokalerService)
        {
            _ledigeLokalerService = ledigeLokalerService;
        }
        
        public void OnGet()
        {
            _lokaleListe = _ledigeLokalerService.GetAll();
            LokaleData = new List<LokaleData>(_lokaleListe);
            IClaimsPrincipal icp = User as IClaimsPrincipal;

            //IClaimsIdentity claimsIdentity = (IClaimsIdentity) icp.Identity;

            foreach (var claim in User.Claims)
            {
                if (claim.Value == "jepp198l@zealand.dk")
                {
                    TestA = claim.Value;
                }
                if (claim.Value == "marx3796@zealand.dk")
                {
                    TestB = claim.Value;
                }

                if (claim.Value == "alex650w@zealand.dk")
                {
                    TestC = claim.Value;
                }
            }
        }

        public IActionResult OnPost()
        {
            foreach (var claim in User.Claims)
            {
                if (claim.Value == "jepp198l@zealand.dk")
                {
                    TestA = claim.Value;
                    return RedirectToPage("/AdministrationPages/AdministrationBooking");
                }
                if (claim.Value == "marx3796@zealand.dk")
                {
                    TestB = claim.Value;
                    return RedirectToPage("/UnderviserPages/UnderviserBooking");
                }
                if (claim.Value == "alex650w@zealand.dk")
                {
                    TestC = claim.Value;
                    return RedirectToPage("/StuderendePages/StuderendeBooking");
                }
            }

            return Page();
        }
    }
}
