using DopravniPodnikSem.Repository.Interfaces;
using DopravniPodnikSem.Services;
using DopravniPodnikSem.Views;
using Oracle.ManagedDataAccess.Client;
using DopravniPodnikSem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Repository
{
    public class SystemKatalogRepository : ISystemKatalogRepository
    {
        private readonly DatabaseService _databaseService;

        public SystemKatalogRepository(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<IEnumerable<SystemKatalog>> GetSystemKatalogAsync()
        {
            var katalogList = new List<SystemKatalog>();

            var query = @"
                SELECT 
                    OWNER, 
                    OBJECT_NAME, 
                    OBJECT_ID, 
                    TO_CHAR(CREATED, 'YYYY-MM-DD HH24:MI:SS') AS CREATED, 
                    STATUS, 
                    OBJECT_TYPE, 
                    TO_CHAR(LAST_DDL_TIME, 'DD.MM.YY') AS LAST_DDL_TIME
                FROM 
                    ALL_OBJECTS
                WHERE 
                    OWNER = 'ST69588'
                ORDER BY 
                    OBJECT_NAME";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var katalog = new SystemKatalog
                        {
                            OWNER = reader.GetString(reader.GetOrdinal("OWNER")),
                            OBJECT_NAME = reader.GetString(reader.GetOrdinal("OBJECT_NAME")),
                            OBJECT_ID = reader.GetInt32(reader.GetOrdinal("OBJECT_ID")),
                            CREATED = reader.GetString(reader.GetOrdinal("CREATED")),
                            STATUS = reader.IsDBNull(reader.GetOrdinal("STATUS")) ? null : reader.GetString(reader.GetOrdinal("STATUS")),
                            OBJECT_TYPE = reader.GetString(reader.GetOrdinal("OBJECT_TYPE")),
                            LAST_DDL_TIME = reader.GetString(reader.GetOrdinal("LAST_DDL_TIME"))
                        };

                        katalogList.Add(katalog);
                    }
                }
            }

            return katalogList;
        }
    }
}


