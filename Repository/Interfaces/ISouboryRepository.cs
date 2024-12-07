using DopravniPodnikSem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Repository.Interfaces
{
    public interface ISouboryRepository
    {
        Task<Soubory> GetUserAvatarAsync(int souborId);
        Task<int> UpdateUserAvatarAsync(int userId, string avatarName, byte[] avatarData);
    }
}
