using DopravniPodnikSem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Repository.Interfaces
{
    public interface ILinkyRepository
    {
        Task<IEnumerable<Linka>> GetAllAsync();
        Task AddAsync(Linka linka);
        Task UpdateAsync(Linka linka);
        Task DeleteAsync(int linkaId);
    }
}
