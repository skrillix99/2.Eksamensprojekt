using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuperBookerData;

namespace _2.Eksamensprojekt.Services
{
    public interface ILedigeLokalerService
    {
        List<LokaleData> GetAll();
        List<LokaleData> GetAllLokaleBySqlString(string sql);
    }
}