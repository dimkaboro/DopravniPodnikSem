using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DopravniPodnikSem.Models;

namespace DopravniPodnikSem.Repository.Interfaces
{
    public interface IUserDataRepository
    {
        Task<Zamestnanec> CheckCredentialsAsync(string email, string password);
        Task RegisterNewUserAsync(Zamestnanec zamestnanec);
        Task<int> AddAddressAsync(string mesto, string ulice, string cisloBudovy, string zipCode, string cisloBytu);
        Task<int> GetAddressIdAsync(string mesto, string ulice, string cisloBudovy, string zipCode, string cisloBytu);
        Task AddEmployeeAsync(string jmeno, string prijmeni, string email, string heslo, string cisloTelefonu, int adresa);
    }

    //Task<Zamestnanec> GetUserEmailByUserId(int id);
    //Task<Zamestnanec> GetUserByUserId(int id);
}

