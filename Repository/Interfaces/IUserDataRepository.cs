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
        //Task<int> RegisterNewUserData(NetworkCredential cred);
        //Task UpdateUserEmail(Zamestnanec zamestnanec);
        //void UpdateUserPassword(Zamestnanec zamestnanec, NetworkCredential pass);

        //Task<Zamestnanec> GetUserEmailByUserId(int id);
        //Task<Zamestnanec> GetUserByUserId(int id);
    }
}
