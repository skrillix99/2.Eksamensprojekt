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

        public int brugerID
        {
            get => _brugerID;
            set => brugerID = value;
        }

        public string brugerNavn
        {
            get => _brugerNavn;
            set => brugerNavn = value;
        }

        public string brugerTlf
        {
            get => _brugerTlf;
            set => brugerTlf = value;
        }

        public string brugerEmail
        {
            get => _brugerEmail;
            set => brugerEmail = value;
        }

        public string brugerPassword
        {
            get => _brugerPassword;
            set => brugerPassword = value;
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
