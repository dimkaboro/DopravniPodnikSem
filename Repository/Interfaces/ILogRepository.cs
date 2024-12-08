﻿using DopravniPodnikSem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Repository.Interfaces
{
    public interface ILogRepository
    {
        Task<IEnumerable<Log>> GetAllAsync();
        Task<Log> GetByTabulkaAsync(string uzivatel);
        Task DeleteAsync(int logId);

    }
}