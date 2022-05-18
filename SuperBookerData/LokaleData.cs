using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperBookerData
{
    public enum LokaleSize { Mødelokale, Klasselokale, Auditorium }
    public class LokaleData
    {
        private int _lokaleID;
        private string _lokaleNavn;
        private string _lokaleNummer;
        private bool _lokaleSmartBoard;
        private int _muligeBookinger;
        private int _etage;

        public LokaleSize LokaleSize
        {
            get;
            set;
        }

        public int LokaleID
        {
            get => _lokaleID;
            set => _lokaleID = value;
        }

        public string LokaleNavn
        {
            get => _lokaleNavn;
            set => _lokaleNavn = value;
        }

        public string LokaleNummer
        {
            get => _lokaleNummer;
            set => _lokaleNummer = value;
        }

        public bool LokaleSmartBoard
        {
            get => _lokaleSmartBoard;
            set => _lokaleSmartBoard = value;
        }

        public int MuligeBookinger
        {
            get => _muligeBookinger;
            set => _muligeBookinger = value;
        }

        public int Etage
        {
            get => _etage;
            set => _etage = value;
        }
        public LokaleData()
        {
        }

        public LokaleData(int LokaleID, string LokaleNavn, string LokaleNummer, bool LokaleSmartBoard, int MuligeBookinger, LokaleSize size, int etage)
        {
            _lokaleID = LokaleID;
            _lokaleNavn = LokaleNavn;
            _lokaleNummer = LokaleNummer;
            _lokaleSmartBoard = LokaleSmartBoard;
            _muligeBookinger = MuligeBookinger;
            LokaleSize = size;
            _etage = etage;
        }

        public override string ToString()
        {
            return $"{nameof(LokaleSize)}: {LokaleSize}, {nameof(LokaleID)}: {LokaleID}, {nameof(LokaleNavn)}: {LokaleNavn}, {nameof(LokaleNummer)}: {LokaleNummer}, {nameof(LokaleSmartBoard)}: {LokaleSmartBoard}, {nameof(MuligeBookinger)}: {MuligeBookinger}";
        }
    }
}