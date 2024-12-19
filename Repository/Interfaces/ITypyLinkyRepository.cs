using DopravniPodnikSem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Repository.Interfaces
{
    public interface ITypyLinkyRepository
    {
        Task<IEnumerable<TypLinky>> GetAllAsync();
    }
}
