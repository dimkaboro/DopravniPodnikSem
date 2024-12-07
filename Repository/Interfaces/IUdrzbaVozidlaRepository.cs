using DopravniPodnikSem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Repository.Interfaces
{
    public interface IUdrzbaVozidlaRepository
    {
        Task<IEnumerable<UdrzbaVozidla>> GetAllAsync();
        Task<IEnumerable<UdrzbaVozidla>> GetAllByDateAsync(DateTime date);
        Task AddAsync(UdrzbaVozidla udrzba, int vozidloId);
        Task UpdateAsync(UdrzbaVozidla udrzba, int vozidloId);
        Task DeleteAsync(int udrzbaId, int vozidloId);
    }
}
