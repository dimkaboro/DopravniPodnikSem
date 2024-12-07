using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository.Interfaces;
using DopravniPodnikSem.Services;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Repository
{
    public class SouboryRepository : ISouboryRepository
    {
        private readonly DatabaseService _databaseService;

        public SouboryRepository(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }


        public async Task<Soubory> GetUserAvatarAsync(int souborId)
        {
            using (var connection = _databaseService.GetConnection())
            {
                var command = new OracleCommand(@"
                    SELECT SOUBOR_ID, NAZEV, SOUBOR 
                    FROM SOUBORY 
                    WHERE SOUBOR_ID = :SouborId", connection);

                command.Parameters.Add(new OracleParameter(":SouborId", souborId));

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Soubory
                        {
                            SouborId = reader.GetInt32(reader.GetOrdinal("SOUBOR_ID")),
                            Nazev = reader.GetString(reader.GetOrdinal("NAZEV")),
                            Soubor = reader.GetValue(reader.GetOrdinal("SOUBOR")) as byte[]
                        };
                    }
                }
            }
            return null;
        }

        public async Task<int> UpdateUserAvatarAsync(int userId, string avatarName, byte[] avatarData)
        {
            using (var connection = _databaseService.GetConnection())
            {
                var command = new OracleCommand("InsertAvatarAndUpdateUser", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add(new OracleParameter("p_AvatarNazev", OracleDbType.Varchar2) { Value = avatarName });
                command.Parameters.Add(new OracleParameter("p_AvatarData", OracleDbType.Blob) { Value = avatarData });
                command.Parameters.Add(new OracleParameter("p_UserId", OracleDbType.Decimal) { Value = userId });

                var newAvatarIdParam = new OracleParameter("p_NewAvatarId", OracleDbType.Decimal)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(newAvatarIdParam);

                await command.ExecuteNonQueryAsync();

                var oracleDecimalValue = (Oracle.ManagedDataAccess.Types.OracleDecimal)newAvatarIdParam.Value;
                return oracleDecimalValue.ToInt32();
            }
        }
    }
}
