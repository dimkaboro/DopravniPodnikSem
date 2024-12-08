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
        Task AddEmployeeAsync(string jmeno, string prijmeni, string email, string heslo, string cisloTelefonu, int adresa);
        Task<Zamestnanec> GetUserDetailsAsync(int userId);
        Task UpdateEmployeeAsync(Zamestnanec employee);
        Task<IEnumerable<Zamestnanec>> GetAllUsersAsync();
        Task<List<Zamestnanec>> GetEmployeeHierarchyAsync();


        Task<IEnumerable<Zamestnanec>> GetAllAsync();
        Task UpdateAsync(Zamestnanec zamestnanec);
        Task DeleteAsync(int zamestnanecId);
    }
}

