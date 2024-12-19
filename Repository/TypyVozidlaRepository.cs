using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository.Interfaces;
using DopravniPodnikSem.Services;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Repository
{
    public class TypyVozidlaRepository : ITypyVozidlaRepository
    {
        private readonly DatabaseService _databaseService;

        public TypyVozidlaRepository(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<IEnumerable<TypVozidla>> GetAllAsync()
        {
            var typyVozidla = new List<TypVozidla>();
            var query = "SELECT TYPVOZIDLA_ID, TYP FROM TYPY_VOZIDLA";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        typyVozidla.Add(new TypVozidla
                        {
                            TypVozidlaId = reader.GetInt32(0),
                            Typ = reader.GetString(1)
                        });
                    }
                }
            }

            return typyVozidla;
        }
    }
}
