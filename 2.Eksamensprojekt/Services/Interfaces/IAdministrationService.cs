using System.Collections.Generic;
using System.Data.SqlClient;
using SuperBookerData;

namespace _2.Eksamensprojekt.Services.Interfaces
{
    public interface IAdministrationService
    {
        List<LokaleData> GetAllLokaler();
        LokaleData GetSingelLokale(int id);
    }
}