﻿using DopravniPodnikSem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Repository.Interfaces
{
    public interface IRidiciRepository
    {
        Task<IEnumerable<Ridic>> GetAllAsync();
        Task<IEnumerable<Ridic>> GetByLastNameAsync(string prijmeni); 
        Task AddAsync(Ridic ridic);
        Task UpdateAsync(Ridic ridic);
        Task DeleteAsync(int ridicId);
        Task<List<string>> GetBirthdaysInCurrentMonthAsync();
    }
}
