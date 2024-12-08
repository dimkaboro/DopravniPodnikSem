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
    public class LinkyRepository : ILinkyRepository
    {
        private readonly DatabaseService _databaseService;

        public string ErrorMessage { get; private set; }

        public LinkyRepository(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<IEnumerable<Linka>> GetAllAsync()
        {
            var linky = new List<Linka>();
            var query = "SELECT * FROM LINKY";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    linky.Add(new Linka
                    {
                        LinkaId = reader.GetInt32(0),
                        Nazev = reader.GetString(1),
                        Typ = reader.GetString(2)
                    });
                }
            }

            return linky;
        }

        public async Task AddAsync(Linka linka)
        {
            var query = "BEGIN manage_linka('INSERT', :LinkaId, :Nazev, :Typ); END;";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":LinkaId", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.InputOutput,
                    Value = DBNull.Value 
                });
                command.Parameters.Add(new OracleParameter(":Nazev", linka.Nazev));
                command.Parameters.Add(new OracleParameter(":Typ", linka.Typ));

                await command.ExecuteNonQueryAsync();

                linka.LinkaId = Convert.ToInt32(((OracleDecimal)command.Parameters[":LinkaId"].Value).Value);
            }
        }

        public async Task UpdateAsync(Linka linka)
        {
            var query = "BEGIN manage_linka('UPDATE', :LinkaId, :Nazev, :Typ); END;";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":LinkaId", linka.LinkaId));
                command.Parameters.Add(new OracleParameter(":Nazev", linka.Nazev));
                command.Parameters.Add(new OracleParameter(":Typ", linka.Typ));

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteAsync(int linkaId)
        {
            var query = "DELETE FROM LINKY WHERE LINKA_ID = :LinkaId";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":LinkaId", linkaId));

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
