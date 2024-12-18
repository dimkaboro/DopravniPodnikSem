using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository.Interfaces;
using DopravniPodnikSem.Services;
using DopravniPodnikSem.ViewModels;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DopravniPodnikSem.ViewModels;

namespace DopravniPodnikSem.Repository
{
    public class LogRepository : ILogRepository
    {
        private readonly DatabaseService _databaseService;
        public string ErrorMessage { get; private set; }

        public LogRepository(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<IEnumerable<Log>> GetAllAsync()
        {
            var logy = new List<Log>();
            var query = "SELECT * FROM LOGY ORDER BY LOG_ID ASC";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    logy.Add(new Log
                    {
                        LogId = reader.GetInt32(reader.GetOrdinal("LOG_ID")),
                        JakaTabulka = reader.GetString(reader.GetOrdinal("JAKA_TABULKA")),
                        Operace = reader.GetString(reader.GetOrdinal("OPERACE")),
                        CasOperace = reader.GetDateTime(reader.GetOrdinal("CAS_OPERACE")),
                        Uzivatel = reader.GetString(reader.GetOrdinal("UZIVATEL")),
                        Popis = reader.GetString(reader.GetOrdinal("POPIS")),
                        OldValues = reader.IsDBNull(reader.GetOrdinal("OLD_VALUES"))
                                    ? null : reader.GetString(reader.GetOrdinal("OLD_VALUES")),
                        NewValues = reader.IsDBNull(reader.GetOrdinal("NEW_VALUES"))
                                    ? null : reader.GetString(reader.GetOrdinal("NEW_VALUES"))
                    });
                }
            }

            return logy;
        }


        public async Task<Log> GetByTabulkaAsync(string jakaTabulka)
        {
            var query = "SELECT * FROM LOGY WHERE JAKA_TABULKA = :JakaTabulka";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":JakaTabulka", jakaTabulka));

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Log
                        {
                            LogId = reader.GetInt32(0),
                            JakaTabulka = reader.GetString(1),
                            Operace = reader.GetString(2),
                            CasOperace = reader.GetDateTime(3),
                            Uzivatel = reader.GetString(4),
                            Popis = reader.GetString(5)
                        };
                    }
                }
            }

            return null;
        }

        public async Task DeleteAsync(int logId)
        {
            var query = "BEGIN DeleteLog(:LogId); END;";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":LogId", logId));
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}





