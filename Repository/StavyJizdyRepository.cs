using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository.Interfaces;
using DopravniPodnikSem.Services;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Repository
{
    public class StavyJizdyRepository : IStavyJizdyRepository
    {
        private readonly DatabaseService _databaseService;

        public StavyJizdyRepository(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<IEnumerable<StavJizdy>> GetAllAsync()
        {
            var stavyJizdy = new List<StavJizdy>();
            var query = "SELECT STAVJIZDY_ID, STAV FROM STAVY_JIZDY";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        stavyJizdy.Add(new StavJizdy
                        {
                            StavJizdyId = reader.GetInt32(0),
                            Stav = reader.GetString(1)
                        });
                    }
                }
            }

            return stavyJizdy;
        }
    }
}
