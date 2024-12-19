using DopravniPodnikSem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Repository.Interfaces
{
    public interface IVozidloRepository
    {
        Task<IEnumerable<Vozidlo>> GetAllAsync();
        Task<Vozidlo> GetByRegistrationNumberAsync(string registracniCislo);
        Task AddAsync(Vozidlo vozidlo);
        Task UpdateAsync(Vozidlo vozidlo);
        Task DeleteAsync(int vozidloId);

        Task<List<(string VehicleType, string CapacityRange, int Count)>> GetVehicleCountsByCapacityAsync();


    }
}
