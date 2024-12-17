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
    public class DopravniPlatbyRepository : IDopravniPlatbyRepository
    {
        private readonly DatabaseService _databaseService;

        public DopravniPlatbyRepository(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<IEnumerable<DopravniPlatba>> GetAllAsync()
        {
            var platby = new List<DopravniPlatba>();
            var query = "SELECT BILET_ID, CENA, DATUM_NAKUPU, TYP_PLATBY, JIZDA_JIZDA_ID FROM DOPRAVNI_PLATBY";

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
                        platby.Add(new DopravniPlatba
                        {
                            BiletId = reader.GetInt32(0),
                            Cena = reader.GetDecimal(1),
                            DatumNakupu = reader.GetDateTime(2),
                            TypPlatby = reader.IsDBNull(3) ? null : reader.GetString(3),
                            JizdaJizdaId = reader.GetInt32(4)
                        });
                    }
                }
            }

            return platby;
        }

        public async Task<string> GetMostFrequentPaymentTypeAsync()
        {
            var query = "BEGIN :result := GetMostFrequentPaymentType; END;";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":result", OracleDbType.Varchar2, 100)
                {
                    Direction = ParameterDirection.Output
                });

                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                await command.ExecuteNonQueryAsync();

                return command.Parameters[":result"].Value.ToString();
            }
        }


        public async Task<IEnumerable<DopravniPlatba>> GetByDateAsync(DateTime searchDate)
        {
            var platby = new List<DopravniPlatba>();
            var query = "SELECT BILET_ID, CENA, DATUM_NAKUPU, TYP_PLATBY, JIZDA_JIZDA_ID " +
                        "FROM DOPRAVNI_PLATBY WHERE TRUNC(DATUM_NAKUPU) = :SearchDate";

            using (var connection = _databaseService.GetConnection())
            {
                using (var command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(new OracleParameter(":SearchDate", OracleDbType.Date) { Value = searchDate });

                    if (connection.State != ConnectionState.Open)
                    {
                        await connection.OpenAsync();
                    }

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            platby.Add(new DopravniPlatba
                            {
                                BiletId = reader.GetInt32(0),
                                Cena = reader.GetDecimal(1),
                                DatumNakupu = reader.GetDateTime(2),
                                TypPlatby = reader.GetString(3),
                                JizdaJizdaId = reader.GetInt32(4)
                            });
                        }
                    }
                }
            }

            return platby;
        }

        public async Task AddAsync(DopravniPlatba dopravniPlatba)
        {
            var query = "BEGIN manage_dopravni_platba('INSERT', :BiletId, :Cena, :DatumNakupu, :TypPlatby, :JizdaId); END;";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":BiletId", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.InputOutput,
                    Value = DBNull.Value
                });
                command.Parameters.Add(new OracleParameter(":Cena", dopravniPlatba.Cena));
                command.Parameters.Add(new OracleParameter(":DatumNakupu", dopravniPlatba.DatumNakupu));
                command.Parameters.Add(new OracleParameter(":TypPlatby", string.IsNullOrEmpty(dopravniPlatba.TypPlatby) ? (object)DBNull.Value : dopravniPlatba.TypPlatby));
                command.Parameters.Add(new OracleParameter(":JizdaId", dopravniPlatba.JizdaJizdaId));

                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                await command.ExecuteNonQueryAsync();

                if (command.Parameters[":BiletId"].Value is OracleDecimal oracleDecimal)
                {
                    dopravniPlatba.BiletId = oracleDecimal.ToInt32();
                }
                else
                {
                    throw new InvalidCastException("Unable to cast returned BiletId to OracleDecimal.");
                }
            }
        }

        public async Task<(int totalCount, decimal totalSum)> CalculatePaymentSummaryAsync()
        {
            var query = "BEGIN Calculate_Payment_Summary(:p_total_count, :p_total_sum); END;";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":p_total_count", OracleDbType.Int32, ParameterDirection.Output));
                command.Parameters.Add(new OracleParameter(":p_total_sum", OracleDbType.Decimal, ParameterDirection.Output));

                await command.ExecuteNonQueryAsync();

                int totalCount = ((OracleDecimal)command.Parameters[":p_total_count"].Value).ToInt32();
                decimal totalSum = ((OracleDecimal)command.Parameters[":p_total_sum"].Value).Value;

                return (totalCount, totalSum);
            }
        }

        public async Task UpdateAsync(DopravniPlatba dopravniPlatba)
        {
            var query = "BEGIN manage_dopravni_platba('UPDATE', :BiletId, :Cena, :DatumNakupu, :TypPlatby, :JizdaId); END;";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":BiletId", dopravniPlatba.BiletId));
                command.Parameters.Add(new OracleParameter(":Cena", dopravniPlatba.Cena));
                command.Parameters.Add(new OracleParameter(":DatumNakupu", dopravniPlatba.DatumNakupu));
                command.Parameters.Add(new OracleParameter(":TypPlatby", string.IsNullOrEmpty(dopravniPlatba.TypPlatby) ? (object)DBNull.Value : dopravniPlatba.TypPlatby));
                command.Parameters.Add(new OracleParameter(":JizdaId", dopravniPlatba.JizdaJizdaId));

                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<(string Type, int Count, double Percentage)> GetMostFrequentPaymentTypeWithDetailsAsync()
        {
            var query = "SELECT Type, Count, Percentage FROM MostFrequentPaymentTypeView";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return (
                            Type: reader.GetString(reader.GetOrdinal("Type")),
                            Count: reader.GetInt32(reader.GetOrdinal("Count")),
                            Percentage: reader.GetDouble(reader.GetOrdinal("Percentage"))
                        );
                    }
                }
            }

            throw new Exception("No payment type data found.");
        }

        public async Task DeleteAsync(int platbaId)
        {
            var query = "DELETE FROM DOPRAVNI_PLATBY WHERE BILET_ID = :BiletId";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":BiletId", platbaId));

                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
