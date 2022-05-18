using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using _2.Eksamensprojekt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuperBookerData;

namespace _2.Eksamensprojekt.Pages.Shared
{
    [Authorize(Roles = "Underviser, Administration")]

    public class BookedeLokalerModel : PageModel
    {
        private ILokalerService _ledigeLokalerService;
        private IBookingService _bookingService;

        private static List<BookingData> _bookingListe;

        private static List<BookingData> _lokaleListe;

        public List<BookingData> LokaleData { get; private set; }
        public List<string> SKEtage { get; private set; }
        public List<string> SKStoerrelse { get; private set; }

        [BindProperty]
        public string SKEtage_valg { get; set; }

        [BindProperty]
        public string SKStoerrelse_valg { get; set; }

        [BindProperty]
        public List<BookingData> BookingData { get; set; }


        public BookedeLokalerModel(IBookingService bookingService, ILokalerService ledigeLokalerService)
        {
            _bookingService = bookingService;
            _ledigeLokalerService = ledigeLokalerService;
            SKEtage = new List<string>()
            {
                "Vælg etage", "Stueplan (D1)", "1. etage (D2)", "2. etage (D3)"
            };
            SKStoerrelse = new List<string>()
            {
               "Vælg lokale størrelse", "Gruppelokaler", "Klasselokaler", "Auditorie"
            };
        }

        public void OnGet()
        {
            _bookingListe = _bookingService.GetAllBookings();
            LokaleData = new List<BookingData>(_bookingListe);
        }
        public void OnPost()
        {           
             LokaleData = new List<BookingData>();
            string sql = "Select Dag, TidSlut, BookesFor, BrugerRolle, BrugerEmail, LokaleEtage, " +
                "BrugerNavn, LokaleNavn, LokaleNummer, LokaleSmartBoard, Size " +
                "From Reservation INNER JOIN Person ON Reservation.BrugerID_FK = Person.BrugerID " +
                "INNER JOIN Lokale ON Reservation.LokaleID_FK = Lokale.LokaleID " +
                "INNER JOIN LokaleLokation ON Lokale.LokaleLokation_FK = LokaleLokation.LokaleLokationId " +
                "INNER JOIN LokaleSize ON Lokale.LokaleSize_FK = LokaleSize.SizeId " + 
                "WHERE 1=1 ";
            //  "Vælg Etage"
            if (SKEtage_valg == "Stueplan (D1)")
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

            // "Vælg lokale størrelse"
            if (SKStoerrelse_valg == "Gruppelokaler")
            {
                sql += "AND Size = 0";
            }
            if (SKStoerrelse_valg == "Klasselokaler")
            {
                sql += "AND Size = 1";
            }
            if (SKStoerrelse_valg == "Auditorie")
            {
                sql += "AND Size = 2";
            }

            LokaleData = _ledigeLokalerService.GetAllLokaleBySqlStringBooking(sql);
        }
    }
}

