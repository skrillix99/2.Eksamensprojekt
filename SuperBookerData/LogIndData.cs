using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperBookerData
{
    public enum brugerRolle { Student, Underviser, Administration };
    public class LogIndData
    {
        public string EmailLogInd { get; set; } //bruger email
        public string Password { get; set; }
        public brugerRolle rolle { get; set; }


        public bool Studerende { get { return rolle == brugerRolle.Student; } }
        public bool Underviser { get { return rolle == brugerRolle.Underviser; } }
        public bool Administration { get { return rolle == brugerRolle.Administration; } }

        public LogIndData() : this("", "") { }

        public LogIndData(string EmailLogInd, string Password) :this(EmailLogInd, Password, brugerRolle.Student)
        {

        }

        public LogIndData(string emailLogInd, string password, brugerRolle role)
        {
            EmailLogInd = emailLogInd;
            Password = password;
            rolle = role;
        }
    }
}
