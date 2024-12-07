using DopravniPodnikSem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Repository.Interfaces
{
    public interface IJizdaRepository
    {
        Task<IEnumerable<Jizda>> GetAllAsync();
        Task<IEnumerable<Jizda>> GetByDateAsync(DateTime casOd); // Новый метод для поиска по дате
        Task AddAsync(Jizda jizda);
        Task UpdateAsync(Jizda jizda);
        Task DeleteAsync(int jizdaId);
        Task UpdateStatusesAsync(); // Вызов процедуры для обновления статусов
        Task<string> CalculateDurationAsync(int jizdaId); // Вызов функции для расчета длительности
    }
}
