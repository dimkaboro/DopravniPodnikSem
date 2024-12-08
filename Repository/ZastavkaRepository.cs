using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository.Interfaces;
using DopravniPodnikSem.Services;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Repository
{
    public class ZastavkaRepository : IZastavkaRepository
    {
        private readonly DatabaseService _databaseService;
        private Zastavka zastavka;

        public ZastavkaRepository(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public ZastavkaRepository(Zastavka zastavka)
        {
            this.zastavka = zastavka;
        }

        public async Task<IEnumerable<Zastavka>> GetAllAsync()
        {
            var zastavky = new List<Zastavka>();
            var query = "SELECT ZASTAVKA_ID, NAZEV, GPS_SOURADNICE FROM ZASTAVKY ORDER BY ZASTAVKA_ID";

            try
            {
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
                            zastavky.Add(new Zastavka
                            {
                                ZastavkaId = reader.IsDBNull(0) ? 0 : ((OracleDecimal)reader.GetOracleDecimal(0)).ToInt32(),
                                Nazev = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                GpsSouradnice = reader.IsDBNull(2) ? string.Empty : reader.GetString(2)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка выполнения GetAllAsync: {ex.Message}");
                throw; // Перебрасываем исключение для обработки выше
            }

            return zastavky;
        }

        public async Task<Zastavka> GetByNazevAsync(string nazev)
        {
            var query = "SELECT ZASTAVKA_ID, NAZEV, GPS_SOURADNICE FROM ZASTAVKY WHERE NAZEV = :Nazev";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":Nazev", OracleDbType.Varchar2)
                {
                    Value = nazev
                });

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Zastavka
                        {
                            ZastavkaId = ((OracleDecimal)reader.GetValue(0)).ToInt32(),
                            Nazev = reader.GetString(1),
                            GpsSouradnice = reader.GetString(2)
                        };
                    }
                }
            }

            return null; // Если запись не найдена
        }

        public async Task AddAsync(Zastavka zastavka)
        {
            var query = "BEGIN manage_zastavka('INSERT', :ZastavkaId, :Nazev, :GpsSouradnice); END;";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":ZastavkaId", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.InputOutput,
                    Value = DBNull.Value // Новый ID будет возвращен процедурой
                });
                command.Parameters.Add(new OracleParameter(":Nazev", OracleDbType.Varchar2)
                {
                    Value = zastavka.Nazev
                });
                command.Parameters.Add(new OracleParameter(":GpsSouradnice", OracleDbType.Varchar2)
                {
                    Value = zastavka.GpsSouradnice
                });

                await command.ExecuteNonQueryAsync();

                // Преобразуем возвращенный ID
                zastavka.ZastavkaId = ((OracleDecimal)command.Parameters[":ZastavkaId"].Value).ToInt32();
            }
        }

        public async Task UpdateAsync(Zastavka zastavka)
        {
            var query = "BEGIN manage_zastavka('UPDATE', :ZastavkaId, :Nazev, :GpsSouradnice); END;";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":ZastavkaId", OracleDbType.Int32)
                {
                    Value = zastavka.ZastavkaId
                });
                command.Parameters.Add(new OracleParameter(":Nazev", OracleDbType.Varchar2)
                {
                    Value = zastavka.Nazev
                });
                command.Parameters.Add(new OracleParameter(":GpsSouradnice", OracleDbType.Varchar2)
                {
                    Value = zastavka.GpsSouradnice
                });

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteAsync(int zastavkaId)
        {
            var query = "DELETE FROM ZASTAVKY WHERE ZASTAVKA_ID = :ZastavkaId";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":ZastavkaId", OracleDbType.Int32)
                {
                    Value = zastavkaId
                });

                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}

