using System.Collections.Generic;
using SuperBookerData;

namespace _2.Eksamensprojekt.Services.Interfaces
{
    public interface IPersonService
    {
        public List<LogIndData> GetPersoner();
        PersonData GetSingelPersonByEmail(string email);
    }
}