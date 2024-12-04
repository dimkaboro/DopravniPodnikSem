
using DopravniPodnikSem.Repository.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DopravniPodnikSem.Models;
using DopravniPodnikSem.Services;
using DopravniPodnikSem.Models.Enum;

namespace DopravniPodnikSem.Repository
{
    public class UserDataRepository : IUserDataRepository
    {
        private readonly DatabaseService _databaseService;
        private readonly PasswordService _passwordService;

        public UserDataRepository(DatabaseService databaseService, PasswordService passwordService)
        {
            _databaseService = databaseService;
            _passwordService = passwordService;
        }

        public async Task<int> AddAddressAsync(string mesto, string ulice, string cisloBudovy, string zipCode, string cisloBytu)
        {
            return await _databaseService.AddAddressAsync(mesto, ulice, cisloBudovy, zipCode, cisloBytu);
        }

        public async Task AddEmployeeAsync(string jmeno, string prijmeni, string email, string heslo, string cisloTelefonu, int adresa)
        {
            await _databaseService.AddEmployeeAsync(jmeno, prijmeni, email, heslo, cisloTelefonu, adresa);
        }

        public async Task<Zamestnanec> CheckCredentialsAsync(string email, string password)
        {
            using (var connection = _databaseService.GetConnection())
            {
                var command = new OracleCommand(@"
            SELECT 
                z.zamestnanec_id, 
                z.jmeno, 
                z.prijmeni, 
                z.email, 
                z.heslo, 
                z.pozice,
                r.role_id, 
                r.nazev AS role_name
            FROM zamestnanci z
            INNER JOIN role r ON z.role_role_id = r.role_id
            WHERE z.email = :Email", connection);

                command.Parameters.Add(new OracleParameter(":Email", email));

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        string storedHash = reader.GetString(reader.GetOrdinal("heslo"));
                        if (_passwordService.VerifyPassword(password, storedHash))
                        {
                            return new Zamestnanec
                            {
                                ZamestnanecId = reader.GetInt32(reader.GetOrdinal("zamestnanec_id")),
                                Jmeno = reader.GetString(reader.GetOrdinal("jmeno")),
                                Prijmeni = reader.GetString(reader.GetOrdinal("prijmeni")),
                                Email = reader.GetString(reader.GetOrdinal("email")),
                                Pozice = reader.GetString(reader.GetOrdinal("pozice")),
                                Role = (Role)reader.GetInt32(reader.GetOrdinal("role_id")) // Преобразование ID роли в enum Role
                            };
                        }
                    }
                }
            }
            return null;
        }

        public async Task RegisterNewUserAsync(Zamestnanec zamestnanec)
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        zamestnanec.Heslo = _passwordService.HashPassword(zamestnanec.Heslo);

                        int addressId = await AddAddressAsync(
                            zamestnanec.Adresa.Mesto,
                            zamestnanec.Adresa.Ulice,
                            zamestnanec.Adresa.CisloBudovy,
                            zamestnanec.Adresa.ZipCode,
                            zamestnanec.Adresa.CisloBytu
                        );
                        zamestnanec.AdresaId = addressId;

                        await AddEmployeeAsync(
                            zamestnanec.Jmeno,
                            zamestnanec.Prijmeni,
                            zamestnanec.Email,
                            zamestnanec.Heslo,
                            zamestnanec.CisloTelefonu,
                            zamestnanec.AdresaId
                        );

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task<Zamestnanec> GetUserDetailsAsync(int userId)
        {
            using (var connection = _databaseService.GetConnection())
            {
                var command = new OracleCommand(@"
                    SELECT ZAMESTNANEC_ID, JMENO, PRIJMENI, EMAIL, POZICE, CISLO_TELEFONU, DATUM_NASTUPU, ADRESA_ADRESA_ID, SOUBOR_SOUBOR_ID
                    FROM ZAMESTNANCI 
                    WHERE ZAMESTNANEC_ID = :UserId", connection);

                command.Parameters.Add(new OracleParameter(":UserId", userId));

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Zamestnanec
                        {
                            ZamestnanecId = reader.GetInt32(reader.GetOrdinal("ZAMESTNANEC_ID")),
                            Jmeno = reader.GetString(reader.GetOrdinal("JMENO")),
                            Prijmeni = reader.GetString(reader.GetOrdinal("PRIJMENI")),
                            Email = reader.GetString(reader.GetOrdinal("EMAIL")),
                            Pozice = reader.GetString(reader.GetOrdinal("POZICE")),
                            CisloTelefonu = reader.GetString(reader.GetOrdinal("CISLO_TELEFONU")),
                            DatumNastupu = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("DATUM_NASTUPU"))),
                            AdresaId = reader.GetInt32(reader.GetOrdinal("ADRESA_ADRESA_ID")),
                            SouborId = reader.GetInt32(reader.GetOrdinal("SOUBOR_SOUBOR_ID"))   
                        };
                    }
                }
            }
            return null;
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

        public async Task<byte[]> GetUserAvatarAsync(int souborId)
        {
            using (var connection = _databaseService.GetConnection())
            {
                var command = new OracleCommand(@"
            SELECT SOUBOR 
            FROM SOUBORY 
            WHERE SOUBOR_ID = :SouborId", connection);

                command.Parameters.Add(new OracleParameter(":SouborId", souborId));

                var result = await command.ExecuteScalarAsync();
                return result == null || result == DBNull.Value ? null : (byte[])result;
            }
        }


        //        private readonly string _connectionString;

        //        public UserDataRepository(string connectionString)
        //        {
        //            _connectionString = connectionString;
        //        }

        //        public async Task<UserData> CheckCredentials(NetworkCredential cred)
        //        {
        //            using (var db = new OracleConnection(_connectionString))
        //            {
        //                var parameters = new DynamicParameters();
        //                parameters.Add("p_email", cred.UserName, DbType.String);
        //                parameters.Add("p_password", cred.Password, DbType.String);
        //                parameters.Add("p_success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
        //                parameters.Add("p_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
        //                parameters.Add("p_binary", dbType: DbType.Int32, direction: ParameterDirection.Output);
        //                parameters.Add("p_role", dbType: DbType.Int32, direction: ParameterDirection.Output);

        //                await db.ExecuteAsync("check_username_password", parameters, commandType: CommandType.StoredProcedure);
        //                if (parameters.Get<bool>("p_success"))
        //                {
        //                    return new UserData
        //                    {
        //                        Email = cred.UserName,
        //                        UserId = parameters.Get<int>("p_id"),
        //                        IdContent = parameters.Get<int>("p_binary"),
        //                        RoleUser = parameters.Get<int>("p_role").ToString()
        //                    };
        //                }
        //                return null;
        //            }
        //        }

        //        public async Task<int> RegisterNewUserData(NetworkCredential cred)
        //        {
        //            using (var db = new OracleConnection(_connectionString))
        //            {
        //                var parameters = new DynamicParameters();
        //                parameters.Add("p_email", cred.UserName, DbType.String);
        //                parameters.Add("p_password", cred.Password, DbType.String);
        //                parameters.Add("p_success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
        //                parameters.Add("p_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

        //                await db.ExecuteAsync("register_new_user", parameters, commandType: CommandType.StoredProcedure);
        //                if (parameters.Get<bool>("p_success"))
        //                {
        //                    return parameters.Get<int>("p_id");
        //                }
        //                throw new Exception($"User with email {cred.UserName} already exists");
        //            }
        //        }

        //        public async Task UpdateUserEmail(UserData user)
        //        {
        //            using (var db = new OracleConnection(_connectionString))
        //            {
        //                var parameters = new DynamicParameters();
        //                parameters.Add("p_id_user", user.UserId, DbType.Int32);
        //                parameters.Add("p_email", user.Email, DbType.String);

        //                await db.ExecuteAsync("UPDATE_USER_DATA", parameters, commandType: CommandType.StoredProcedure);
        //            }
        //        }

        //        public void UpdateUserPassword(UserData user, NetworkCredential pass)
        //        {
        //            using (var db = new OracleConnection(_connectionString))
        //            {
        //                var parameters = new DynamicParameters();
        //                parameters.Add("p_id", user.UserId, DbType.Int32);
        //                parameters.Add("p_password", pass.Password, DbType.String);

        //                db.Execute("UPDATE_USER_PASS", parameters, commandType: CommandType.StoredProcedure);
        //            }
        //        }

        //        public async Task<UserData> GetUserEmailByUserId(int id)
        //        {
        //            using (var db = new OracleConnection(_connectionString))
        //            {
        //                return await db.QueryFirstOrDefaultAsync<UserData>(
        //                    "SELECT ID_USER AS UserId, Email FROM USER_DATA WHERE ID_USER = :id",
        //                    new { id });
        //            }
        //        }

        //        public async Task<UserData> GetUserByUserId(int id)
        //        {
        //            using (var db = new OracleConnection(_connectionString))
        //            {
        //                return await db.QueryFirstOrDefaultAsync<UserData>(
        //                    "SELECT ID_USER AS UserId, Email, ID_ROLE AS RoleUser, ID_CONTENT AS IdContent FROM USER_DATA WHERE ID_USER = :id",
        //                    new { id });
        //            }
        //        }
    }
}
