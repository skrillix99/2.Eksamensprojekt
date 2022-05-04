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
        private int _brugerId;

        public int BrugerID_FK
        {
            get => _brugerId;
            set => BrugerID_FK = value;
        }

        public TimeSpan TidSlut
        {
            get => _tidSlut;
            set => TidSlut = value;
        }
        
        public TimeSpan TidStart
        {
            get => _tidStart;
            set => TidStart = value;
        }

        public DateTime Dag
        {
            get => _dag;
            set => Dag = value;
        }

        public int HeltBooket
        {
            get => _heltBooket;
            set => HeltBooket = value;
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
