using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperBookerData
{
    public class BookingData : PersonData
    {
        private int _resevertionID;
        private TimeSpan _tidStart;
        private DateTime _dag;
        private int _heltBooket;
        private PersonData _bruger;
        private LokaleData _lokale;
        private TimeSpan _tidSlut;
        private brugerRolle _bookesFor;

        public BookingData()
        {
        }

        public BookingData(int resevertionId, TimeSpan tidStart, DateTime dag, int heltBooket, PersonData bruger, LokaleData lokale, TimeSpan tidSlut, brugerRolle rolle)
        {
            _resevertionID = resevertionId;
            _tidStart = tidStart;
            _dag = dag;
            _heltBooket = heltBooket;
            _bruger = bruger;
            _lokale = lokale;
            _tidSlut = tidSlut;
            _bookesFor = rolle;
        }

        public int ResevertionId
        {
            get => _resevertionID;
            set => _resevertionID = value;
        }

        [Required]
        public TimeSpan TidStart
        {
            get => _tidStart;
            set => _tidStart = value;
        }

        [Required]
        public DateTime Dag
        {
            get => _dag;
            set => _dag = value;
        }

        [Required]
        public int HeltBooket
        {
            get => _heltBooket;
            set => _heltBooket = value;
        }

        public PersonData Bruger
        {
            get => _bruger;
            set => _bruger = value;
        }

        public LokaleData Lokale
        {
            get => _lokale;
            set => _lokale = value;
        }

        [Required]
        public TimeSpan TidSlut
        {
            get => _tidSlut;
            set => _tidSlut = value;
        }

        public brugerRolle BookesFor
        {
            get => _bookesFor;
            set => _bookesFor = value;
        }
    }
}