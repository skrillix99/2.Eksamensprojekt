using SuperBookerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace _2.Eksamensprojekt.Services
{
    public interface IBookingService
    {
        public List<LokaleData> GetAll();

        public LokaleData GetRoom(string lokaleNavn);

        public LokaleData GetNumber(string lokaleNummer);

        //TODO public BookingData CreateBooking(????);

    }
}
