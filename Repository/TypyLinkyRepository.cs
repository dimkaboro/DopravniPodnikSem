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
    public class TypyLinkyRepository : ITypyLinkyRepository
    {
        private readonly DatabaseService _databaseService;

        public TypyLinkyRepository(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<IEnumerable<TypLinky>> GetAllAsync()
        {
            var typyLinky = new List<TypLinky>();
            var query = "SELECT TYPLINKY_ID, TYP FROM TYPY_LINKY";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        typyLinky.Add(new TypLinky
                        {
                            TypLinkyId = reader.GetInt32(0),
                            Typ = reader.GetString(1)
                        });
                    }
                }
            }

            return typyLinky;
        }
    }
}
