using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperBookerData
{
    public enum LokaleSize { S, M, L}
    public class LokaleData
    {
        private int _lokaleID;
        private string _lokaleNavn;
        private string _lokaleNummer;
        private bool _lokaleSmartBoard;
        private int _lokaleMuligeBookinger;

        public LokaleSize LokaleSize
        {
            get;
        }

        public int lokaleID
        {
            get => _lokaleID;
            set => lokaleID = value;
        }

        public string lokaleNavn
        {
            get => _lokaleNavn;
            set => lokaleNavn = value;
        }

        public string lokaleNummer
        {
            get => _lokaleNummer;
            set => lokaleNummer = value;
        }

        public bool lokaleSmartBoard
        {
            get => _lokaleSmartBoard;
            set => lokaleSmartBoard = value;
        }

        public int lokaleMuligeBookinger
        {
            get => _lokaleMuligeBookinger;
            set => lokaleMuligeBookinger = value;
        }

        public LokaleData()
        {

        }

        public LokaleData(int lokaleID, string lokaleNavn, string lokaleNummer, bool lokaleSmartBoard, int lokaleMuligeBookinger)
        {
            _lokaleID = lokaleID;
            _lokaleNavn = lokaleNavn;
            _lokaleNummer = lokaleNummer;
            _lokaleSmartBoard = lokaleSmartBoard;
            _lokaleMuligeBookinger = lokaleMuligeBookinger;
            LokaleSize = 0;
        }

    }
}
