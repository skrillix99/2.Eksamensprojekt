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
        // enum LokaleSize 
        private int _lokaleMuligeBookinger;

        public LokaleData()
        {
        }

        public LokaleData(int lokaleId, string lokaleNavn, string lokaleNummer, bool lokaleSmartBoard, int muligeBookinger)
        {
            _lokaleID = lokaleId;
            _lokaleNavn = lokaleNavn;
            _lokaleNummer = lokaleNummer;
            _lokaleSmartBoard = lokaleSmartBoard;
            _lokaleMuligeBookinger = muligeBookinger;
        }

        public int lokaleID
        {
            get => _lokaleID;
            set => _lokaleID = value;
        }

        public string lokaleNavn
        {
            get => _lokaleNavn;
            set => _lokaleNavn = value;
        }

        public string lokaleNummer
        {
            get => _lokaleNummer;
            set => _lokaleNummer = value;
        }

        public bool lokaleSmartBoard
        {
            get => _lokaleSmartBoard;
            set => _lokaleSmartBoard = value;
        }

        public LokaleSize lokaleSize
        {
            get;
            set;
        }

        public int lokaleMuligeBookinger
        {
            get => _lokaleMuligeBookinger;
            set => _lokaleMuligeBookinger = value;
        }
    }
}
