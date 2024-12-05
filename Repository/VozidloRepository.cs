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
                        GarazeGarazId = reader.GetInt32(4),
                        UdrzbaVozidlaUdrzbaId = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5) // Обработка NULL
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
                            GarazeGarazId = reader.GetInt32(4),
                            UdrzbaVozidlaUdrzbaId = reader.GetInt32(5)
                        };
                    }
                }
            }

            return null;
        }

        public async Task AddAsync(Vozidlo vozidlo)
        {
            var query = "BEGIN AddVozidlo(:RegistracniCislo, :Typ, :Kapacita, :GarazId); END;";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                // Проверяем параметры
                command.Parameters.Add(new OracleParameter(":RegistracniCislo", vozidlo.RegistracniCislo));
                command.Parameters.Add(new OracleParameter(":Typ", vozidlo.Typ));
                command.Parameters.Add(new OracleParameter(":Kapacita", vozidlo.Kapacita ?? (object)DBNull.Value));
                command.Parameters.Add(new OracleParameter(":GarazId", vozidlo.GarazeGarazId));

                try
                {
                    await command.ExecuteNonQueryAsync();
                   
                }
                catch (OracleException ex)
                {
                    throw new InvalidOperationException($"Ошибка при добавлении транспортного средства: {ex.Message}", ex);
                }
            }
        }


        public async Task UpdateAsync(Vozidlo vozidlo)
        {
            var query = @"
                UPDATE VOZIDLA
                SET REGISTRACNI_CISLO = :RegistracniCislo, TYP = :Typ, KAPACITA = :Kapacita,
                    GARAZ_GARAZ_ID = :GarazeGarazId, UDRZBA_VOZIDLA_UDRZBA_ID = :UdrzbaVozidlaUdrzbaId
                WHERE VOZIDLO_ID = :VozidloId";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":RegistracniCislo", vozidlo.RegistracniCislo));
                command.Parameters.Add(new OracleParameter(":Typ", vozidlo.Typ));
                command.Parameters.Add(new OracleParameter(":Kapacita", vozidlo.Kapacita ?? (object)DBNull.Value));
                command.Parameters.Add(new OracleParameter(":GarazeGarazId", vozidlo.GarazeGarazId));
                command.Parameters.Add(new OracleParameter(":UdrzbaVozidlaUdrzbaId", vozidlo.UdrzbaVozidlaUdrzbaId));
                command.Parameters.Add(new OracleParameter(":VozidloId", vozidlo.VozidloId));

                try
                {
                    await command.ExecuteNonQueryAsync();
                    
                }
                catch (OracleException ex)
                {
                    throw new InvalidOperationException($"Ошибка при добавлении транспортного средства: {ex.Message}", ex);
                }
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
