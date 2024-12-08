using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository.Interfaces;
using DopravniPodnikSem.Services;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Repository
{
    public class JizdaRepository : IJizdaRepository
    {
        private readonly DatabaseService _databaseService;

        public JizdaRepository(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<IEnumerable<Jizda>> GetAllAsync()
        {
            var jizdy = new List<Jizda>();
            var query = "SELECT JIZDA_ID, CAS_OD, CAS_DO, STAV, LINKA_LINKA_ID, RIDIC_RIDIC_ID, VOZIDLO_VOZIDLO_ID FROM JIZDY";

            using (var connection = _databaseService.GetConnection())
            {
                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                using (var command = new OracleCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        jizdy.Add(new Jizda
                        {
                            JizdaId = reader.GetInt32(0),
                            CasOd = reader.GetDateTime(1),
                            CasDo = reader.GetDateTime(2),
                            Stav = reader.GetString(3),
                            LinkaId = reader.GetInt32(4),
                            RidicId = reader.GetInt32(5),
                            VozidloId = reader.GetInt32(6)
                        });
                    }
                }
            }

            return jizdy;
        }

        public async Task<IEnumerable<Jizda>> GetByDateAsync(DateTime casOd)
        {
            var jizdy = new List<Jizda>();
            var query = "SELECT JIZDA_ID, CAS_OD, CAS_DO, STAV, LINKA_LINKA_ID, RIDIC_RIDIC_ID, VOZIDLO_VOZIDLO_ID FROM JIZDY WHERE TRUNC(CAS_OD) = :CasOd";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":CasOd", casOd));

                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        jizdy.Add(new Jizda
                        {
                            JizdaId = reader.GetInt32(0),
                            CasOd = reader.GetDateTime(1),
                            CasDo = reader.GetDateTime(2),
                            Stav = reader.GetString(3),
                            LinkaId = reader.GetInt32(4),
                            RidicId = reader.GetInt32(5),
                            VozidloId = reader.GetInt32(6)
                        });
                    }
                }
            }

            Console.WriteLine($"Records found: {jizdy.Count}");
            return jizdy;
        }

        public async Task AddAsync(Jizda jizda)
        {
            var query = "BEGIN manage_jizda('INSERT', :JizdaId, :CasOd, :CasDo, :Stav, :LinkaId, :RidicId, :VozidloId); END;";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":JizdaId", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.InputOutput,
                    Value = DBNull.Value 
                });
                command.Parameters.Add(new OracleParameter(":CasOd", jizda.CasOd));
                command.Parameters.Add(new OracleParameter(":CasDo", jizda.CasDo));
                command.Parameters.Add(new OracleParameter(":Stav", jizda.Stav));
                command.Parameters.Add(new OracleParameter(":LinkaId", jizda.LinkaId));
                command.Parameters.Add(new OracleParameter(":RidicId", jizda.RidicId));
                command.Parameters.Add(new OracleParameter(":VozidloId", jizda.VozidloId));

                await command.ExecuteNonQueryAsync();

                jizda.JizdaId = Convert.ToInt32(((OracleDecimal)command.Parameters[":JizdaId"].Value).Value);
            }
        }

        public async Task UpdateAsync(Jizda jizda)
        {
            var query = "BEGIN manage_jizda('UPDATE', :JizdaId, :CasOd, :CasDo, :Stav, :LinkaId, :RidicId, :VozidloId); END;";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":JizdaId", jizda.JizdaId));
                command.Parameters.Add(new OracleParameter(":CasOd", jizda.CasOd));
                command.Parameters.Add(new OracleParameter(":CasDo", jizda.CasDo));
                command.Parameters.Add(new OracleParameter(":Stav", jizda.Stav));
                command.Parameters.Add(new OracleParameter(":LinkaId", jizda.LinkaId));
                command.Parameters.Add(new OracleParameter(":RidicId", jizda.RidicId));
                command.Parameters.Add(new OracleParameter(":VozidloId", jizda.VozidloId));

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteAsync(int jizdaId)
        {
            var query = "DELETE FROM JIZDY WHERE JIZDA_ID = :JizdaId";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":JizdaId", jizdaId));

                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<(string LinkaNazev, DateTime CasOd, DateTime CasDo, TimeSpan Duration)> GetLongestJizdaAsync()
        {
            var query = "BEGIN :result_cursor := GetLongestJizda(); END;";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":result_cursor", OracleDbType.RefCursor) { Direction = ParameterDirection.Output });

                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                await command.ExecuteNonQueryAsync();

                using (var reader = ((OracleRefCursor)command.Parameters[":result_cursor"].Value).GetDataReader())
                {
                    if (await reader.ReadAsync())
                    {
                        return (
                            LinkaNazev: reader.GetString(0),
                            CasOd: reader.GetDateTime(1),
                            CasDo: reader.GetDateTime(2),
                            Duration: reader.GetTimeSpan(3)
                        );
                    }
                }
            }

            throw new Exception("No data found for the longest jizda.");
        }

        public async Task UpdateStatusesAsync()
        {
            var query = "BEGIN UpdateAllJizdaStatuses(); END;";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<string> CalculateDurationAsync(int jizdaId)
        {
            var query = "BEGIN :result := CalculateDuration(:jizdaId); END;";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":result", OracleDbType.Varchar2, 50) { Direction = ParameterDirection.Output });
                command.Parameters.Add(new OracleParameter(":jizdaId", jizdaId));

                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                await command.ExecuteNonQueryAsync();

                var result = command.Parameters[":result"].Value;
                return result != null ? result.ToString() : "No duration calculated.";
            }
        }
    }
}

