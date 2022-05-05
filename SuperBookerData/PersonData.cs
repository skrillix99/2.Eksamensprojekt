using System;

namespace SuperBookerData
{
    public class PersonData
    {
        private int _brugerID;
        private string _brugerNavn;
        private string _brugerTlf;
        private string _brugerEmail;
        private string _brugerPassword;

        public brugerRolle brugerRolle
        {
            get;
        }

        public int brugerID
        {
            get => _brugerID;
            set => _brugerID = value;
        }

        public string BrugerNavn
        {
            get => _brugerNavn;
            set => _brugerNavn = value;
        }

        public string brugerTlf
        {
            get => _brugerTlf;
            set => _brugerTlf = value;
        }

        public string brugerEmail
        {
            get => _brugerEmail;
            set => _brugerEmail = value;
        }

        public string brugerPassword
        {
            get => _brugerPassword;
            set => _brugerPassword = value;
        }

        public PersonData()
        {

        }

        public PersonData(int brugerID, string brugerNavn, string brugerTlf, string brugerEmail, string brugerPassword)
        {
            _brugerID = brugerID;
            _brugerNavn = brugerNavn;
            _brugerTlf = brugerTlf;
            _brugerEmail = brugerEmail;
            _brugerPassword = brugerPassword;
        }

    }
}