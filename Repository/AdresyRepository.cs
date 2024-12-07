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
    public class AdresyRepository : IAdresyRepository
    {
        private readonly DatabaseService _databaseService;

        public AdresyRepository(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }


        public async Task<int> AddAddressAsync(string mesto, string ulice, string cisloBudovy, string zipCode, string cisloBytu)
        {
            using (var connection = _databaseService.GetConnection())
            {
                var insertCommand = new OracleCommand(@"
                    INSERT INTO ADRESY (ADRESA_ID, MESTO, ULICE, CISLO_BUDOVY, ZIP_CODE, CISLO_BYTU)
                    VALUES ((SELECT NVL(MAX(ADRESA_ID), 0) + 1 FROM ADRESY), :Mesto, :Ulice, :CisloBudovy, :ZipCode, :CisloBytu)
                    RETURNING ADRESA_ID INTO :NewId", connection);

                insertCommand.Parameters.Add(new OracleParameter(":Mesto", mesto));
                insertCommand.Parameters.Add(new OracleParameter(":Ulice", ulice));
                insertCommand.Parameters.Add(new OracleParameter(":CisloBudovy", cisloBudovy));
                insertCommand.Parameters.Add(new OracleParameter(":ZipCode", zipCode));
                insertCommand.Parameters.Add(new OracleParameter(":CisloBytu", cisloBytu));

                var newIdParameter = new OracleParameter(":NewId", OracleDbType.Int32) { Direction = ParameterDirection.Output };
                insertCommand.Parameters.Add(newIdParameter);

                await insertCommand.ExecuteNonQueryAsync();

                if (newIdParameter.Value is OracleDecimal oracleDecimal)
                {
                    return oracleDecimal.ToInt32();
                }
                else
                {
                    throw new InvalidOperationException("Failed to insert address and retrieve ID.");
                }
            }
        }

        public async Task<Adresa> GetAddressDetailsAsync(int adresaId)
        {
            using (var connection = _databaseService.GetConnection())
            {
                var command = new OracleCommand(@"
                    SELECT ADRESA_ID, MESTO, ULICE, CISLO_BUDOVY, ZIP_CODE, CISLO_BYTU 
                    FROM ADRESY 
                    WHERE ADRESA_ID = :AdresaId", connection);

                command.Parameters.Add(new OracleParameter(":AdresaId", adresaId));

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Adresa
                        {
                            AdresaId = reader.GetInt32(reader.GetOrdinal("ADRESA_ID")),
                            Mesto = reader.GetString(reader.GetOrdinal("MESTO")),
                            Ulice = reader.GetString(reader.GetOrdinal("ULICE")),
                            CisloBudovy = reader.GetString(reader.GetOrdinal("CISLO_BUDOVY")),
                            ZipCode = reader.GetString(reader.GetOrdinal("ZIP_CODE")),
                            CisloBytu = reader.GetString(reader.GetOrdinal("CISLO_BYTU"))
                        };
                    }
                }
            }
            return null;
        }

        public async Task<int> UpdateAddressLogicAsync(Adresa address, int zamestnanecId, int currentAddressId)
        {
            using (var connection = _databaseService.GetConnection())
            {
                var command = new OracleCommand("UpdateAddressLogic", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add(new OracleParameter("p_Mesto", OracleDbType.Varchar2) { Value = address.Mesto });
                command.Parameters.Add(new OracleParameter("p_Ulice", OracleDbType.Varchar2) { Value = address.Ulice });
                command.Parameters.Add(new OracleParameter("p_CisloBudovy", OracleDbType.Varchar2) { Value = address.CisloBudovy });
                command.Parameters.Add(new OracleParameter("p_ZipCode", OracleDbType.Varchar2) { Value = address.ZipCode });
                command.Parameters.Add(new OracleParameter("p_CisloBytu", OracleDbType.Varchar2) { Value = address.CisloBytu });
                command.Parameters.Add(new OracleParameter("p_ZamestnanecId", OracleDbType.Decimal) { Value = zamestnanecId });
                command.Parameters.Add(new OracleParameter("p_CurrentAddressId", OracleDbType.Decimal) { Value = currentAddressId });

                var outputParam = new OracleParameter("p_NewAddressId", OracleDbType.Decimal)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputParam);

                await command.ExecuteNonQueryAsync();

                // Используем явное преобразование через OracleDecimal
                var oracleDecimalValue = (Oracle.ManagedDataAccess.Types.OracleDecimal)outputParam.Value;
                return oracleDecimalValue.ToInt32(); // Преобразуем в Int32
            }
        }



        public async Task<int> GetAddressIdAsync(string mesto, string ulice, string cisloBudovy, string zipCode, string cisloBytu)
        {
            using (var connection = _databaseService.GetConnection())
            {
                var command = new OracleCommand(@"
                    SELECT ADRESA_ID FROM ADRESY 
                    WHERE MESTO = :Mesto AND ULICE = :Ulice AND CISLO_BUDOVY = :CisloBudovy AND ZIP_CODE = :ZipCode AND CISLO_BYTU = :CisloBytu", connection);

                command.Parameters.Add(new OracleParameter(":Mesto", mesto));
                command.Parameters.Add(new OracleParameter(":Ulice", ulice));
                command.Parameters.Add(new OracleParameter(":CisloBudovy", cisloBudovy));
                command.Parameters.Add(new OracleParameter(":ZipCode", zipCode));
                command.Parameters.Add(new OracleParameter(":CisloBytu", cisloBytu));

                var result = await command.ExecuteScalarAsync();

                try
                {
                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to get address.");
                }
            }
        }
    }
}
