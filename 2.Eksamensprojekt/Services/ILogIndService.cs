using SuperBookerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2.Eksamensprojekt.Services
{
    interface ILogIndService
    {
        public bool Contains(LogIndData LogInd);

        public brugerRolle ContainsAndGiveRole(LogIndData LogInd);
    }
}
