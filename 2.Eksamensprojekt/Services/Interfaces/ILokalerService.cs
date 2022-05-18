using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuperBookerData;

namespace _2.Eksamensprojekt.Services
{
    public interface ILokalerService
    {
        List<LokaleData> GetAllLokaler();
        List<LokaleData> GetAllLokaleBySqlString(string sql);

        /// <summary>
        /// Henter et enkelt lokale fra Lokale tabel, baseret på det parameter der bliver sendt med.
        /// </summary>
        /// <param name="id">Typen int. Må ikke være negativt</param>
        /// <returns>et object af typen LokaleData</returns>
        LokaleData GetSingelLokale(int id);
    }
}