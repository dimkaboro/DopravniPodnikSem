using DopravniPodnikSem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Repository.Interfaces
{
    public interface IZastavkaRepository
    {
        Task<IEnumerable<Zastavka>> GetAllAsync();
        Task<Zastavka> GetByNazevAsync(string nazev);
        Task AddAsync(Zastavka zastavka);
        Task UpdateAsync(Zastavka zastavka);
        Task DeleteAsync(int zastavkaId);
    }
}
