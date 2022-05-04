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
        private int _muligeBookinger;

        public LokaleSize LokaleSize
        {
            get;
            set;
        }

        public int LokaleID
        {
            get => _lokaleID;
            set => LokaleID = value;
        }

        public string LokaleNavn
        {
            get => _lokaleNavn;
            set => LokaleNavn = value;
        }

        public string LokaleNummer
        {
            get => _lokaleNummer;
            set => LokaleNummer = value;
        }

        public bool LokaleSmartBoard
        {
            get => _lokaleSmartBoard;
            set => LokaleSmartBoard = value;
        }

        public int MuligeBookinger
        {
            get => _muligeBookinger;
            set => MuligeBookinger = value;
        }

        public LokaleData()
        {

        }

        public LokaleData(int LokaleID, string LokaleNavn, string LokaleNummer, bool LokaleSmartBoard, int MuligeBookinger)
        {
            _lokaleID = LokaleID;
            _lokaleNavn = LokaleNavn;
            _lokaleNummer = LokaleNummer;
            _lokaleSmartBoard = LokaleSmartBoard;
            _muligeBookinger = MuligeBookinger;
            LokaleSize = 0;
        }

    }
}
