using SuperBookerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2.Eksamensprojekt.Services
{
    public class LogIndService
    {
        private readonly List<LogIndData> _logIndData;

        public bool Contains(LogIndData LogInd)
        {
            return _logIndData.Contains(LogInd);
        }

        public brugerRolle ContainsAndGiveRole(LogIndData LogInd)
        {
            if (!Contains(LogInd))
            {
                LogInd.rolle = brugerRolle.Student;
            }
            else
            {
                LogInd.rolle = (LogInd.EmailLogInd.Equals("email")) ? brugerRolle.Underviser : brugerRolle.Administration;
            }

            return LogInd.rolle;
        }

    }
}
