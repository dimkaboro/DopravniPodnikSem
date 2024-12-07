using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository.Interfaces;
using DopravniPodnikSem.Services;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
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
            var query = "SELECT * FROM ZASTAVKY";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    zastavky.Add(new Zastavka
                    {
                        ZastavkaId = reader.GetInt32(0),
                        Nazev = reader.GetString(1),
                        GpsSouradnice = reader.GetString(2)
                    });
                }
            }

            return zastavky;
        }

        public async Task<Zastavka> GetByIdAsync(int zastavkaId)
        {
            var query = "SELECT * FROM ZASTAVKY WHERE ZASTAVKA_ID = :ZastavkaId";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":ZastavkaId", zastavkaId));

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Zastavka
                        {
                            ZastavkaId = reader.GetInt32(0),
                            Nazev = reader.GetString(1),
                            GpsSouradnice = reader.GetString(2)
                        };
                    }
                }
            }

            return null;
        }

        public async Task AddAsync(Zastavka zastavka)
        {
            var query = "INSERT INTO ZASTAVKY (NAZEV, GPS_SOURADNICE) VALUES (:Nazev, :GpsSouradnice)";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":Nazev", zastavka.Nazev));
                command.Parameters.Add(new OracleParameter(":GpsSouradnice", zastavka.GpsSouradnice));

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateAsync(Zastavka zastavka)
        {
            var query = "UPDATE ZASTAVKY SET NAZEV = :Nazev, GPS_SOURADNICE = :GpsSouradnice WHERE ZASTAVKA_ID = :ZastavkaId";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":Nazev", zastavka.Nazev));
                command.Parameters.Add(new OracleParameter(":GpsSouradnice", zastavka.GpsSouradnice));
                command.Parameters.Add(new OracleParameter(":ZastavkaId", zastavka.ZastavkaId));

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteAsync(int zastavkaId)
        {
            var query = "DELETE FROM ZASTAVKY WHERE ZASTAVKA_ID = :ZastavkaId";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":ZastavkaId", zastavkaId));
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
