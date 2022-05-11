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
        private int _lokaleEtage;

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

        public int LokaleMuligeBookinger
        {
            get => _lokaleMuligeBookinger;
            set => _lokaleMuligeBookinger = value;
        }

        public int LokaleEtage
        {
            get => _lokaleEtage;
            set => _lokaleEtage = value;
        }
        public LokaleData()
        {

        }

        public LokaleData(int lokaleID, string lokaleNavn, string lokaleNummer, bool lokaleSmartBoard, int lokaleMuligeBookinger, int etage)
        {
            _lokaleID = lokaleID;
            _lokaleNavn = lokaleNavn;
            _lokaleNummer = lokaleNummer;
            _lokaleSmartBoard = lokaleSmartBoard;
            _lokaleMuligeBookinger = lokaleMuligeBookinger;
            LokaleSize = 0;
            _lokaleEtage = etage;
        }

    }
}
