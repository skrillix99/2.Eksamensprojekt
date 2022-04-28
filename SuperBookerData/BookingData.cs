using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperBookerData
{
    public class BookingData
    {
        private TimeSpan _tidsrum;
        private DateTime _dag;
        private bool _heltBooket;
        
        public TimeSpan Tidsrum
        {
            get => _tidsrum;
            set => Tidsrum = value;
        }

        public DateTime Dag
        {
            get => _dag;
            set => Dag = value;
        }

        public bool HeltBooket
        {
            get => _heltBooket;
            set => HeltBooket = value;
        }

        public BookingData()
        {

        }

        public BookingData(TimeSpan Tidsrum, DateTime Dag, bool HeltBooket)
        {
            _tidsrum = Tidsrum;
            _dag = Dag;
            _heltBooket = HeltBooket;
        }
    }
}
