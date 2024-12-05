﻿using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository.Interfaces;
using DopravniPodnikSem.Services;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
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
                        DatumNarozeni = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4)
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
                            DatumNarozeni = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4)
                        });
                    }
                }
            }

            return ridici;
        }

        public async Task AddAsync(Ridic ridic)
        {
            var query = @"
INSERT INTO RIDICI (RIDIC_ID, JMENO, PRIJMENI, RIDIC_PRUKAZ, DATUM_NAROZENI)
VALUES ((SELECT NVL(MAX(RIDIC_ID), 0) + 1 FROM RIDICI), :Jmeno, :Prijmeni, :RidicPrukaz, :DatumNarozeni)";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":Jmeno", ridic.Jmeno));
                command.Parameters.Add(new OracleParameter(":Prijmeni", ridic.Prijmeni));
                command.Parameters.Add(new OracleParameter(":RidicPrukaz", ridic.RidicPrukaz));
                command.Parameters.Add(new OracleParameter(":DatumNarozeni", ridic.DatumNarozeni ?? (object)DBNull.Value));

                try
                {
                    await command.ExecuteNonQueryAsync();
                    ErrorMessage = string.Empty;
                }
                catch (OracleException ex)
                {
                    ErrorMessage = $"Ошибка добавления водителя: {ex.Message}";
                }
            }
        }

        public async Task UpdateAsync(Ridic ridic)
        {
            var query = @"
 UPDATE RIDICI
SET JMENO = :Jmeno, PRIJMENI = :Prijmeni, RIDIC_PRUKAZ = :RidicPrukaz, DATUM_NAROZENI = :DatumNarozeni
WHERE RIDIC_ID = :RidicId";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":Jmeno", ridic.Jmeno));
                command.Parameters.Add(new OracleParameter(":Prijmeni", ridic.Prijmeni));
                command.Parameters.Add(new OracleParameter(":RidicPrukaz", ridic.RidicPrukaz));
                command.Parameters.Add(new OracleParameter(":DatumNarozeni", ridic.DatumNarozeni ?? (object)DBNull.Value));
                command.Parameters.Add(new OracleParameter(":RidicId", ridic.RidicId));

                try
                {
                    await command.ExecuteNonQueryAsync();
                    ErrorMessage = string.Empty;
                }
                catch (OracleException ex)
                {
                    ErrorMessage = $"Ошибка обновления водителя: {ex.Message}";
                }
            }
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
                    ErrorMessage = $"Ошибка удаления водителя: {ex.Message}";
                }
            }
        }
    }
}
