using DopravniPodnikSem.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Repository.Interfaces
{
    public interface ISystemKatalogRepository
    {
        Task<IEnumerable<Models.SystemKatalog>> GetSystemKatalogAsync();
    }
}
