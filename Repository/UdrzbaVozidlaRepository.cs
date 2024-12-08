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
    public class UdrzbaVozidlaRepository : IUdrzbaVozidlaRepository
    {
        private readonly DatabaseService _databaseService;

        public UdrzbaVozidlaRepository(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<IEnumerable<UdrzbaVozidla>> GetAllAsync()
        {
            var udrzby = new List<UdrzbaVozidla>();
            var query = "SELECT * FROM UDRZBY_VOZIDLA";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    udrzby.Add(new UdrzbaVozidla
                    {
                        UdrzbaId = reader.GetInt32(0),
                        DatumUdrzby = reader.GetDateTime(1),
                        Popis = reader.GetString(2)
                    });
                }
            }

            return udrzby;
        }

        public async Task<IEnumerable<UdrzbaVozidla>> GetAllByDateAsync(DateTime searchDate)
        {
            var udrzby = new List<UdrzbaVozidla>();
            var query = "SELECT * FROM UDRZBY_VOZIDLA WHERE TRUNC(CAS_UDRZBY) = :SearchDate";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":SearchDate", searchDate.Date));
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        udrzby.Add(new UdrzbaVozidla
                        {
                            UdrzbaId = reader.GetInt32(0),
                            DatumUdrzby = reader.GetDateTime(1),
                            Popis = reader.GetString(2)
                        });
                    }
                }
            }

            return udrzby;
        }

        public async Task AddAsync(UdrzbaVozidla udrzba, int vozidloId)
        {
            var query = "BEGIN manage_udrzba_vozdila('INSERT', :UdrzbaId, :CasUdrzby, :Popis, :VozidloId); END;";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":UdrzbaId", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.InputOutput,
                    Value = DBNull.Value // ID будет создан процедурой
                });
                command.Parameters.Add(new OracleParameter(":CasUdrzby", udrzba.DatumUdrzby));
                command.Parameters.Add(new OracleParameter(":Popis", udrzba.Popis));
                command.Parameters.Add(new OracleParameter(":VozidloId", vozidloId));

                await command.ExecuteNonQueryAsync();

                // Обновление ID записи
                udrzba.UdrzbaId = Convert.ToInt32(((OracleDecimal)command.Parameters[":UdrzbaId"].Value).Value);
            }
        }

        public async Task UpdateAsync(UdrzbaVozidla udrzba, int vozidloId)
        {
            var query = "BEGIN manage_udrzba_vozdila('UPDATE', :UdrzbaId, :CasUdrzby, :Popis, :VozidloId); END;";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":UdrzbaId", udrzba.UdrzbaId));
                command.Parameters.Add(new OracleParameter(":CasUdrzby", udrzba.DatumUdrzby));
                command.Parameters.Add(new OracleParameter(":Popis", udrzba.Popis));
                command.Parameters.Add(new OracleParameter(":VozidloId", vozidloId));

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteAsync(int udrzbaId, int vozidloId)
        {
            var deleteQuery = "DELETE FROM UDRZBY_VOZIDLA WHERE UDRZBA_ID = :UdrzbaId";

            var clearVozidloQuery = "UPDATE VOZIDLA SET UDRZBA_VOZIDLA_UDRZBA_ID = NULL WHERE VOZIDLO_ID = :VozidloId";

            using (var connection = _databaseService.GetConnection())
            using (var deleteCommand = new OracleCommand(deleteQuery, connection))
            using (var clearCommand = new OracleCommand(clearVozidloQuery, connection))
            {
                deleteCommand.Parameters.Add(new OracleParameter(":UdrzbaId", udrzbaId));
                clearCommand.Parameters.Add(new OracleParameter(":VozidloId", vozidloId));

                await clearCommand.ExecuteNonQueryAsync();
                await deleteCommand.ExecuteNonQueryAsync();
            }
        }
    }
}
