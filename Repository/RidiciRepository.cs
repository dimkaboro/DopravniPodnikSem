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
    public class RidiciRepository : IRidiciRepository
    {
        private readonly DatabaseService _databaseService;

        public string ErrorMessage { get; private set; }

        public RidiciRepository(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<IEnumerable<Ridic>> GetAllAsync()
        {
            var ridici = new List<Ridic>();
            var query = "SELECT * FROM RIDICI";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    ridici.Add(new Ridic
                    {
                        RidicId = reader.GetInt32(0),
                        Jmeno = reader.GetString(1),
                        Prijmeni = reader.GetString(2),
                        RidicPrukaz = reader.GetString(3),
                        DatumNarozeni = reader.IsDBNull(4) ? (DateOnly?)null : DateOnly.FromDateTime(reader.GetDateTime(4))
                    });
                }
            }

            return ridici;
        }

        public async Task<IEnumerable<Ridic>> GetByLastNameAsync(string prijmeni)
        {
            var ridici = new List<Ridic>();
            var query = "SELECT * FROM RIDICI WHERE PRIJMENI = :Prijmeni";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":Prijmeni", prijmeni));

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        ridici.Add(new Ridic
                        {
                            RidicId = reader.GetInt32(0),
                            Jmeno = reader.GetString(1),
                            Prijmeni = reader.GetString(2),
                            RidicPrukaz = reader.GetString(3),
                            DatumNarozeni = reader.IsDBNull(4) ? (DateOnly?)null : DateOnly.FromDateTime(reader.GetDateTime(4))
                        });
                    }
                }
            }

            return ridici;
        }

        public async Task AddAsync(Ridic ridic)
        {
            var query = "BEGIN manage_ridic('INSERT', :RidicId, :Jmeno, :Prijmeni, :RidicPrukaz, :DatumNarozeni); END;";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":RidicId", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.InputOutput,
                    Value = DBNull.Value 
                });
                command.Parameters.Add(new OracleParameter(":Jmeno", ridic.Jmeno));
                command.Parameters.Add(new OracleParameter(":Prijmeni", ridic.Prijmeni));
                command.Parameters.Add(new OracleParameter(":RidicPrukaz", ridic.RidicPrukaz));
                command.Parameters.Add(new OracleParameter(":DatumNarozeni", ridic.DatumNarozeni.HasValue ? ridic.DatumNarozeni.Value.ToDateTime(TimeOnly.MinValue) : (object)DBNull.Value));

                await command.ExecuteNonQueryAsync();

                ridic.RidicId = Convert.ToInt32(((OracleDecimal)command.Parameters[":RidicId"].Value).Value);
            }
        }

        public async Task UpdateAsync(Ridic ridic)
        {
            var query = "BEGIN manage_ridic('UPDATE', :RidicId, :Jmeno, :Prijmeni, :RidicPrukaz, :DatumNarozeni); END;";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":RidicId", ridic.RidicId));
                command.Parameters.Add(new OracleParameter(":Jmeno", ridic.Jmeno));
                command.Parameters.Add(new OracleParameter(":Prijmeni", ridic.Prijmeni));
                command.Parameters.Add(new OracleParameter(":RidicPrukaz", ridic.RidicPrukaz));
                command.Parameters.Add(new OracleParameter(":DatumNarozeni", ridic.DatumNarozeni.HasValue ? ridic.DatumNarozeni.Value.ToDateTime(TimeOnly.MinValue) : (object)DBNull.Value));

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<string>> GetBirthdaysInCurrentMonthAsync()
        {
            var birthdays = new List<string>();
            var query = @"BEGIN GetRidiciBirthdaysInCurrentMonth; END;";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.CommandType = CommandType.Text;

                OracleCommand enableDbmsOutput = new OracleCommand("BEGIN DBMS_OUTPUT.ENABLE(); END;", connection);
                await enableDbmsOutput.ExecuteNonQueryAsync();

                await command.ExecuteNonQueryAsync();

                OracleCommand readDbmsOutput = new OracleCommand("BEGIN DBMS_OUTPUT.GET_LINE(:line, :status); END;", connection);
                readDbmsOutput.Parameters.Add("line", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.Output;
                readDbmsOutput.Parameters.Add("status", OracleDbType.Int32).Direction = ParameterDirection.Output;

                while (true)
                {
                    await readDbmsOutput.ExecuteNonQueryAsync();
                    var line = readDbmsOutput.Parameters["line"].Value.ToString();
                    var status = ((OracleDecimal)readDbmsOutput.Parameters["status"].Value).ToInt32();

                    if (status != 0) break; 
                    birthdays.Add(line);
                }
            }

            return birthdays;
        }

        public async Task DeleteAsync(int ridicId)
        {
            var query = "DELETE FROM RIDICI WHERE RIDIC_ID = :RidicId";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":RidicId", ridicId));

                try
                {
                    await command.ExecuteNonQueryAsync();
                    ErrorMessage = string.Empty;
                }
                catch (OracleException ex)
                {
                    ErrorMessage = $"Error: {ex.Message}";
                }
            }
        }
    }
}
