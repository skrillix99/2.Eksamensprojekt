using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperBookerData
{
    public class BookingData : PersonData
    {
        private TimeSpan _tidStart;
        private DateTime _dag;
        private TimeSpan _tidSlut;
        private int _heltBooket;
        private int _brugerID;

        public int BrugerID_FK
        {
            get => _brugerID;
            set => _brugerID = value;
        }

        public TimeSpan TidSlut
        {
            get => _tidSlut;
            set => _tidSlut = value;
        }
        
        public TimeSpan TidStart
        {
            get => _tidStart;
            set => _tidStart = value;
        }

        public DateTime Dag
        {
            get => _dag;
            set => _dag = value;
        }

        public int HeltBooket
        {
            get => _heltBooket;
            set => _heltBooket = value;
        }

        public BookingData()
        {

        }

        public BookingData(TimeSpan TidStart, DateTime Dag, int HeltBooket, TimeSpan TidSlut)
        {
            _tidStart = TidStart;
            _dag = Dag;
            _heltBooket = HeltBooket;
            _tidSlut = TidSlut;
        }
    }
}
