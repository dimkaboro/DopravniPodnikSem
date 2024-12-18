using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository.Interfaces;
using DopravniPodnikSem.Services;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Repository
{
    public class ZastavkyTrasyRepository : IZastavkyTrasyRepository
    {
        private readonly DatabaseService _databaseService;

        public ZastavkyTrasyRepository(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<IEnumerable<ZastavkaTrasa>> GetAllAsync()
        {
            var zastavkyTrasy = new List<ZastavkaTrasa>();
            var query = "BEGIN :result := GetZastavkyTrasyDetail(); END;";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter("result", OracleDbType.RefCursor)
                {
                    Direction = ParameterDirection.Output
                });

                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                await command.ExecuteNonQueryAsync();

                using (var reader = ((OracleRefCursor)command.Parameters["result"].Value).GetDataReader())
                {
                    while (await reader.ReadAsync())
                    {
                        zastavkyTrasy.Add(new ZastavkaTrasa
                        {
                            ZastavkaTrasaId = reader.GetInt32(0),                 
                            CasPrijezdu = reader.GetDateTime(1),                 
                            JizdaDisplay = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            ZastavkaDisplay = reader.IsDBNull(3) ? string.Empty : reader.GetString(3)
                        });
                    }
                }
            }

            return zastavkyTrasy;
        }

        public async Task<IEnumerable<ZastavkaTrasa>> GetByCasPrijezduAsync(DateTime casPrijezdu)
        {
            var zastavkyTrasy = new List<ZastavkaTrasa>();
            var query = @"SELECT * FROM TABLE(GetZastavkyTrasyDetail()) WHERE TRUNC(TO_DATE(CAS_PRIJEZDU, 'DD.MM.YY HH24:MI')) = :CasPrijezdu";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":CasPrijezdu", OracleDbType.Date) { Value = casPrijezdu });

                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        zastavkyTrasy.Add(new ZastavkaTrasa
                        {
                            ZastavkaTrasaId = reader.GetInt32(0),
                            CasPrijezdu = reader.GetDateTime(1),
                            JizdaId = reader.GetInt32(2),
                            ZastavkaId = reader.GetInt32(3),
                            Jizda = new Jizda
                            {
                                CasOd = DateTime.Parse(reader.GetString(4)),
                                CasDo = DateTime.Parse(reader.GetString(5)),
                                Stav = reader.GetString(6),
                                LinkaId = reader.GetInt32(7)
                            },
                            Zastavka = new Zastavka
                            {
                                Nazev = reader.GetString(8),
                                GpsSouradnice = reader.GetString(9)
                            }
                        });
                    }
                }
            }

            return zastavkyTrasy;
        }

        // Добавление записи с использованием процедуры ManageZastavkyTrasyTransaction
        public async Task AddAsync(ZastavkaTrasa zastavkaTrasa)
        {
            var query = @"
        BEGIN 
            ManageZastavkyTrasyTransaction('INSERT', :p_zastavkaTrasaId, :p_casPrijezdu, :p_jizdaId, :p_zastavkaId); 
        END;";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                // Параметр для ID
                var idParameter = new OracleParameter(":p_zastavkaTrasaId", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.InputOutput,
                    Value = DBNull.Value
                };

                command.Parameters.Add(idParameter);
                command.Parameters.Add(new OracleParameter(":p_casPrijezdu", OracleDbType.TimeStamp) { Value = zastavkaTrasa.CasPrijezdu });
                command.Parameters.Add(new OracleParameter(":p_jizdaId", OracleDbType.Int32) { Value = zastavkaTrasa.JizdaId });
                command.Parameters.Add(new OracleParameter(":p_zastavkaId", OracleDbType.Int32) { Value = zastavkaTrasa.ZastavkaId });

                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                await command.ExecuteNonQueryAsync();

                // Конвертация OracleDecimal в Int32
                if (idParameter.Value != DBNull.Value && idParameter.Value is OracleDecimal oracleDecimal)
                {
                    zastavkaTrasa.ZastavkaTrasaId = Convert.ToInt32(oracleDecimal.Value);
                }
            }
        }

        // Обновление записи с использованием процедуры ManageZastavkyTrasyTransaction
        public async Task UpdateAsync(ZastavkaTrasa zastavkaTrasa)
        {
            var query = @"BEGIN ManageZastavkyTrasyTransaction('UPDATE', :Id, :CasPrijezdu, :JizdaId, :ZastavkaId); END;";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":Id", zastavkaTrasa.ZastavkaTrasaId));
                command.Parameters.Add(new OracleParameter(":CasPrijezdu", zastavkaTrasa.CasPrijezdu));
                command.Parameters.Add(new OracleParameter(":JizdaId", zastavkaTrasa.JizdaId));
                command.Parameters.Add(new OracleParameter(":ZastavkaId", zastavkaTrasa.ZastavkaId));

                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                await command.ExecuteNonQueryAsync();
            }
        }

        // Удаление записи по ID
        public async Task DeleteAsync(int zastavkaTrasaId)
        {
            var query = "DELETE FROM ZASTAVKY_TRASY WHERE ZASTAVKATRASA_ID = :ZastavkaTrasaId";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":ZastavkaTrasaId", OracleDbType.Int32)
                {
                    Value = zastavkaTrasaId
                });

                try
                {
                    if (connection.State != ConnectionState.Open)
                        await connection.OpenAsync();

                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    Console.WriteLine($"Rows affected: {rowsAffected}, Deleted ID: {zastavkaTrasaId}");

                    if (rowsAffected == 0)
                        throw new Exception("Запись не найдена в базе данных.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при удалении записи: {ex.Message}");
                    throw;
                }
            }
        }

    }
}
