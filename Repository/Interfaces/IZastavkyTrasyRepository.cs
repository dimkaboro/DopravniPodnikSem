using DopravniPodnikSem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Repository.Interfaces
{
    public interface IZastavkyTrasyRepository
    {
        Task<IEnumerable<ZastavkaTrasa>> GetAllAsync();
        Task<IEnumerable<ZastavkaTrasa>> GetByCasPrijezduAsync(DateTime casPrijezdu);
        Task AddAsync(ZastavkaTrasa zastavkaTrasa);
        Task UpdateAsync(ZastavkaTrasa zastavkaTrasa);
        Task DeleteAsync(int zastavkaTrasaId);
    }
}
