using SuperBookerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2.Eksamensprojekt.Services
{
    public class BookingService : IBookingService
    {
        private readonly List<LokaleData> _lokaleData;

        //public BookingService()
        //{

        //}

        
        public List<LokaleData> GetAll()
        {
            return new List<LokaleData>(_lokaleData);
        }

        public LokaleData GetNumber(string lokaleNummer)
        {
            LokaleData lokale = _lokaleData.Find(x => x.LokaleNummer == lokaleNummer);

            return (lokale != null) ? lokale: throw new KeyNotFoundException();
        }

        public LokaleData GetRoom(string lokaleNavn)
        {
            LokaleData room = _lokaleData.Find(x => x.LokaleNavn == lokaleNavn);

            return (room != null) ? room : throw new KeyNotFoundException();
        }
    }
}
