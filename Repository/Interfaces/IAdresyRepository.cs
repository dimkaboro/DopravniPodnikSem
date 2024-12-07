using DopravniPodnikSem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Repository.Interfaces
{
    public interface IAdresyRepository
    {
        Task<int> AddAddressAsync(string mesto, string ulice, string cisloBudovy, string zipCode, string cisloBytu);
        Task<Adresa> GetAddressDetailsAsync(int adresaId);
        Task<int> UpdateAddressLogicAsync(Adresa address, int zamestnanecId, int currentAddressId);
        Task<int> GetAddressIdAsync(string mesto, string ulice, string cisloBudovy, string zipCode, string cisloBytu);
    }
}
