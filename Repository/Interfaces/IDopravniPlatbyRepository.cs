using DopravniPodnikSem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Repository.Interfaces
{
    public interface IDopravniPlatbyRepository
    {
        Task<IEnumerable<DopravniPlatba>> GetAllAsync();
        Task AddAsync(DopravniPlatba dopravniPlatba);
        Task UpdateAsync(DopravniPlatba dopravniPlatba);
        Task DeleteAsync(int platbaId);
        Task<IEnumerable<DopravniPlatba>> GetByDateAsync(DateTime date);

        Task<string> GetMostFrequentPaymentTypeAsync();
        Task<(int totalCount, decimal totalSum)> CalculatePaymentSummaryAsync();
        Task<(string Type, int Count, double Percentage)> GetMostFrequentPaymentTypeWithDetailsAsync();
    }
}
