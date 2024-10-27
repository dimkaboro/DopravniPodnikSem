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
        Task<UserData> CheckCredentials(NetworkCredential cred);
        Task<int> RegisterNewUserData(NetworkCredential cred);
        Task UpdateUserEmail(UserData user);
        void UpdateUserPassword(UserData user, NetworkCredential pass);

        Task<UserData> GetUserEmailByUserId(int id);
        Task<UserData> GetUserByUserId(int id);
    }
}
