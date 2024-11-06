using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Services
{
    public class DatabaseService
    {
        private readonly IConfiguration _configuration;

        public DatabaseService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> TestConnectionAsync()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            try
            {
                using (var connection = new OracleConnection(connectionString))
                {
                    await connection.OpenAsync();
                    // Если подключение открыто, возвращаем true
                    return connection.State == System.Data.ConnectionState.Open;
                }
            }
            catch (Exception ex)
            {
                // Логирование ошибки (можно заменить на более продвинутый механизм логирования)
                return false;
            }
        }
    }
}