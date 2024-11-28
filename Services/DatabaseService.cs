using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Services
{
    public class DatabaseService : IDisposable
    {
        private readonly IConfiguration _configuration;
        private OracleConnection _connection;

        public DatabaseService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new OracleConnection(_configuration.GetConnectionString("DefaultConnection"));
        }


        public async Task<bool> TestConnectionAsync()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            try
            {
                using (var connection = new OracleConnection(connectionString))
                {
                    await connection.OpenAsync();
                    return connection.State == System.Data.ConnectionState.Open;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public OracleConnection GetConnection()
        {
            if (_connection == null || _connection.State == ConnectionState.Closed || _connection.State == ConnectionState.Broken)
            {
                _connection?.Dispose();
                _connection = new OracleConnection(_configuration.GetConnectionString("DefaultConnection"));
                _connection.Open();
            }
            return _connection;
        }


        public void Dispose()
        {
            _connection?.Dispose();
        }


        public async Task<int> AddAddressAsync(string mesto, string ulice, string cisloBudovy, string zipCode, string cisloBytu)
        {
            using (var connection = GetConnection())
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


        public async Task<int> GetAddressIdAsync(string mesto, string ulice, string cisloBudovy, string zipCode, string cisloBytu)
        {
            using (var connection = GetConnection())
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


        public async Task AddEmployeeAsync(string jmeno, string prijmeni, string email, string heslo, string cisloTelefonu, int adresa)
        {
            using (var connection = GetConnection())
            {
                var command = new OracleCommand(@"
                    INSERT INTO ZAMESTNANCI (ZAMESTNANEC_ID, JMENO, PRIJMENI, EMAIL, HESLO, POZICE, PLAT, DATUM_NASTUPU, CISLO_TELEFONU, 
                    ZAMESTNANEC_ZAMESTNANEC_ID, ADRESA_ADRESA_ID, ROLE_ROLE_ID, SOUBOR_SOUBOR_ID)
                    VALUES ((SELECT NVL(MAX(ZAMESTNANEC_ID), 0) + 1 FROM ZAMESTNANCI), :Jmeno, :Prijmeni, :Email, :Heslo, 'guest', 1, SYSDATE, :CisloTelefonu, 1, :Adresa, 2, 1)", connection);

                command.Parameters.Add(new OracleParameter(":Jmeno", jmeno));
                command.Parameters.Add(new OracleParameter(":Prijmeni", prijmeni));
                command.Parameters.Add(new OracleParameter(":Email", email));
                command.Parameters.Add(new OracleParameter(":Heslo", heslo));
                command.Parameters.Add(new OracleParameter(":CisloTelefonu", cisloTelefonu));
                command.Parameters.Add(new OracleParameter(":Adresa", adresa));

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}