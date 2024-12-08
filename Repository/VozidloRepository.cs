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
    public class VozidloRepository : IVozidloRepository
    {
        private readonly DatabaseService _databaseService;
        public string ErrorMessage { get; private set; }

        public VozidloRepository(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }


        public async Task<IEnumerable<Vozidlo>> GetAllAsync()
        {
            var vozidla = new List<Vozidlo>();
            var query = "SELECT * FROM VOZIDLA";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    vozidla.Add(new Vozidlo
                    {
                        VozidloId = reader.GetInt32(0),
                        RegistracniCislo = reader.GetString(1),
                        Typ = reader.GetString(2),
                        Kapacita = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                        GarazeGarazId = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4),
                        UdrzbaVozidlaUdrzbaId = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5)
                    });
                }
            }

            return vozidla;
        }

        public async Task<Vozidlo> GetByRegistrationNumberAsync(string registracniCislo)
        {
            var query = "SELECT * FROM VOZIDLA WHERE REGISTRACNI_CISLO = :RegistracniCislo";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":RegistracniCislo", registracniCislo));

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Vozidlo
                        {
                            VozidloId = reader.GetInt32(0),
                            RegistracniCislo = reader.GetString(1),
                            Typ = reader.GetString(2),
                            Kapacita = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                            GarazeGarazId = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4),
                            UdrzbaVozidlaUdrzbaId = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5) 
                        };
                    }
                }
            }

            return null;
        }

        public async Task AddAsync(Vozidlo vozidlo)
        {
            var query = "BEGIN manage_vozidlo('INSERT', :VozidloId, :RegistracniCislo, :Typ, :Kapacita, :GarazId); END;";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":VozidloId", OracleDbType.Int32)
                {
                    Direction = System.Data.ParameterDirection.InputOutput,
                    Value = DBNull.Value
                });
                command.Parameters.Add(new OracleParameter(":RegistracniCislo", vozidlo.RegistracniCislo));
                command.Parameters.Add(new OracleParameter(":Typ", vozidlo.Typ));
                command.Parameters.Add(new OracleParameter(":Kapacita", vozidlo.Kapacita ?? (object)DBNull.Value));
                command.Parameters.Add(new OracleParameter(":GarazId", vozidlo.GarazeGarazId ?? (object)DBNull.Value));

                await command.ExecuteNonQueryAsync();

                vozidlo.VozidloId = Convert.ToInt32(command.Parameters[":VozidloId"].Value.ToString());
            }
        }

        public async Task UpdateAsync(Vozidlo vozidlo)
        {
            var query = "BEGIN manage_vozidlo('UPDATE', :VozidloId, :RegistracniCislo, :Typ, :Kapacita, :GarazId); END;";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":VozidloId", vozidlo.VozidloId));
                command.Parameters.Add(new OracleParameter(":RegistracniCislo", vozidlo.RegistracniCislo));
                command.Parameters.Add(new OracleParameter(":Typ", vozidlo.Typ));
                command.Parameters.Add(new OracleParameter(":Kapacita", vozidlo.Kapacita ?? (object)DBNull.Value));
                command.Parameters.Add(new OracleParameter(":GarazId", vozidlo.GarazeGarazId ?? (object)DBNull.Value));

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteAsync(int vozidloId)
        {
            var query = "DELETE FROM VOZIDLA WHERE VOZIDLO_ID = :VozidloId";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":VozidloId", vozidloId));
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}







