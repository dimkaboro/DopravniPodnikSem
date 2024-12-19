
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
                                Role = (Role)reader.GetInt32(reader.GetOrdinal("role_id")) 
                            };
                        }
                    }
                }
            }
            return null;
        }



        public async Task AddEmployeeAsync(Zamestnanec zamestnanec)
        {
            using (var connection = _databaseService.GetConnection())
            {
                var command = new OracleCommand("BEGIN manage_zamestnanec('INSERT', :ZamestnanecId, :Jmeno, :Prijmeni, :Email, :Heslo, :Pozice, :Plat, :DatumNastupu, :CisloTelefonu, :ZamestnanecZamestnanecId, :AdresaId, :RoleId, :SouborId, :JePrivate); END;", connection)
                {
                    CommandType = CommandType.Text
                };

                command.Parameters.Add(new OracleParameter(":ZamestnanecId", DBNull.Value)); 
                command.Parameters.Add(new OracleParameter(":Jmeno", zamestnanec.Jmeno));
                command.Parameters.Add(new OracleParameter(":Prijmeni", zamestnanec.Prijmeni));
                command.Parameters.Add(new OracleParameter(":Email", zamestnanec.Email));
                command.Parameters.Add(new OracleParameter(":Heslo", zamestnanec.Heslo));
                command.Parameters.Add(new OracleParameter(":Pozice", zamestnanec.Pozice));
                command.Parameters.Add(new OracleParameter(":Plat", zamestnanec.Plat));
                command.Parameters.Add(new OracleParameter(":DatumNastupu", zamestnanec.DatumNastupu.ToDateTime(new TimeOnly())));
                command.Parameters.Add(new OracleParameter(":CisloTelefonu", zamestnanec.CisloTelefonu));
                command.Parameters.Add(new OracleParameter(":ZamestnanecZamestnanecId", zamestnanec.ZamestnanecZamestnanecId));
                command.Parameters.Add(new OracleParameter(":AdresaId", zamestnanec.AdresaId));
                command.Parameters.Add(new OracleParameter(":RoleId", zamestnanec.RoleId));
                command.Parameters.Add(new OracleParameter(":SouborId", zamestnanec.SouborId));
                command.Parameters.Add(new OracleParameter(":JePrivate", zamestnanec.JePrivate));

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<Zamestnanec> GetUserDetailsAsync(int userId)
        {
            using (var connection = _databaseService.GetConnection())
            {
                var command = new OracleCommand(@"
                    SELECT ZAMESTNANEC_ID, JMENO, PRIJMENI, EMAIL, POZICE, CISLO_TELEFONU, DATUM_NASTUPU, ADRESA_ADRESA_ID, ROLE_ROLE_ID, SOUBOR_SOUBOR_ID, JE_PRIVATE
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
                            Role = (Role)reader.GetInt32(reader.GetOrdinal("ROLE_ROLE_ID")),
                            SouborId = reader.GetInt32(reader.GetOrdinal("SOUBOR_SOUBOR_ID")),
                            JePrivate = reader.GetInt32(reader.GetOrdinal("JE_PRIVATE"))
                        };
                    }
                }
            }
            return null;
        }

        public async Task UpdateEmployeeAsync(Zamestnanec employee)
        {
            using (var connection = _databaseService.GetConnection())
            {
                var command = new OracleCommand("UpdateEmployee", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add(new OracleParameter("p_Jmeno", OracleDbType.Varchar2) { Value = employee.Jmeno });
                command.Parameters.Add(new OracleParameter("p_Prijmeni", OracleDbType.Varchar2) { Value = employee.Prijmeni });
                command.Parameters.Add(new OracleParameter("p_Email", OracleDbType.Varchar2) { Value = employee.Email });
                command.Parameters.Add(new OracleParameter("p_CisloTelefonu", OracleDbType.Varchar2) { Value = employee.CisloTelefonu });
                command.Parameters.Add(new OracleParameter("p_ZamestnanecId", OracleDbType.Decimal) { Value = employee.ZamestnanecId });
                command.Parameters.Add(new OracleParameter("p_JePrivate", OracleDbType.Decimal) { Value = employee.JePrivate });

                await command.ExecuteNonQueryAsync();
            }
        }




        public async Task<IEnumerable<Zamestnanec>> GetAllUsersAsync()
        {
            using (var connection = _databaseService.GetConnection())
            {
                var command = new OracleCommand("SELECT * FROM ZAMESTNANCI", connection);

                var result = new List<Zamestnanec>();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new Zamestnanec
                        {
                            ZamestnanecId = reader.GetInt32("ZAMESTNANEC_ID"),
                            Jmeno = reader.GetString("JMENO"),
                            Prijmeni = reader.GetString("PRIJMENI"),
                            Email = reader.GetString("EMAIL"),
                            Pozice = reader.GetString("POZICE"),
                            Plat = reader.GetInt32("PLAT"),
                            DatumNastupu = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("DATUM_NASTUPU"))),
                            CisloTelefonu = reader.GetString("CISLO_TELEFONU"),
                            ZamestnanecZamestnanecId = reader.GetInt32("ZAMESTNANEC_ZAMESTNANEC_ID"),
                            AdresaId = reader.GetInt32("ADRESA_ADRESA_ID"),
                            Role = (Role)reader.GetInt32("ROLE_ROLE_ID"),
                            SouborId = reader.GetInt32("SOUBOR_SOUBOR_ID"),
                            JePrivate = reader.GetInt32("JE_PRIVATE")
                        });
                    }
                }
                return result;
            }
        }



        public async Task<List<Zamestnanec>> GetEmployeeHierarchyAsync()
        {
            var employees = new List<Zamestnanec>();
            var employeeMap = new Dictionary<int, Zamestnanec>();

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand("GET_EMPLOYEE_HIERARCHY", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                var cursor = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                command.Parameters.Add(cursor);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var employee = new Zamestnanec
                        {
                            ZamestnanecId = reader.GetInt32(reader.GetOrdinal("ZAMESTNANEC_ID")),
                            Jmeno = reader.GetString(reader.GetOrdinal("JMENO")),
                            Prijmeni = reader.GetString(reader.GetOrdinal("PRIJMENI")),
                            Email = reader.GetString(reader.GetOrdinal("EMAIL")),
                            Pozice = reader.GetString(reader.GetOrdinal("POZICE")),
                            DatumNastupu = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("DATUM_NASTUPU"))),
                            CisloTelefonu = reader.GetString(reader.GetOrdinal("CISLO_TELEFONU")),
                            ZamestnanecZamestnanecId = reader.IsDBNull(reader.GetOrdinal("ZAMESTNANEC_ZAMESTNANEC_ID"))
                                ? (int?)null
                                : reader.GetInt32(reader.GetOrdinal("ZAMESTNANEC_ZAMESTNANEC_ID")),
                            AdresaId = reader.GetInt32(reader.GetOrdinal("ADRESA_ADRESA_ID")),
                            Role = (Role)reader.GetInt32(reader.GetOrdinal("ROLE_ROLE_ID")),
                            SouborId = reader.GetInt32(reader.GetOrdinal("SOUBOR_SOUBOR_ID")),
                            JePrivate = reader.GetInt32(reader.GetOrdinal("JE_PRIVATE"))
                        };

                        employeeMap[employee.ZamestnanecId] = employee;
                        employees.Add(employee);
                    }
                }
            }

            foreach (var employee in employees)
            {
                if (employee.ZamestnanecZamestnanecId.HasValue &&
                    employeeMap.ContainsKey(employee.ZamestnanecZamestnanecId.Value) &&
                    employee.ZamestnanecZamestnanecId != employee.ZamestnanecId)
                {
                    var manager = employeeMap[employee.ZamestnanecZamestnanecId.Value];
                    manager.Podrizeni.Add(employee);
                }
            }

            return employees.Where(e => e.ZamestnanecZamestnanecId == null || e.ZamestnanecZamestnanecId == e.ZamestnanecId).ToList();
        }







        public async Task<IEnumerable<Zamestnanec>> GetAllAsync()
        {
            var zamestnanci = new List<Zamestnanec>();
            var query = "SELECT * FROM ZAMESTNANCI";

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
                        zamestnanci.Add(new Zamestnanec
                        {
                            ZamestnanecId = reader.GetInt32(0),
                            Jmeno = reader.GetString(1),
                            Prijmeni = reader.GetString(2),
                            Email = reader.GetString(3),
                            Heslo = reader.GetString(4),
                            Pozice = reader.GetString(5),
                            Plat = reader.GetInt32(6),
                            DatumNastupu = DateOnly.FromDateTime(reader.GetDateTime(7)),
                            CisloTelefonu = reader.GetString(8),
                            ZamestnanecZamestnanecId = reader.GetInt32(9),
                            AdresaId = reader.GetInt32(10),
                            Role = (Role)reader.GetInt32(11),
                            SouborId = reader.GetInt32(12),
                            JePrivate = reader.GetInt32(13)
                        });
                    }
                }
            }

            return zamestnanci;
        }



        public async Task UpdateAsync(Zamestnanec zamestnanec)
        {
            var query = "BEGIN manage_zamestnanec('UPDATE', :ZamestnanecId, :Jmeno, :Prijmeni, :Email, :Heslo, :Pozice, :Plat, :DatumNastupu, :CisloTelefonu, :ZamestnanecZamestnanecId, :AdresaId, :RoleId, :SouborId, :JePrivate); END;";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":ZamestnanecId", zamestnanec.ZamestnanecId));
                command.Parameters.Add(new OracleParameter(":Jmeno", zamestnanec.Jmeno));
                command.Parameters.Add(new OracleParameter(":Prijmeni", zamestnanec.Prijmeni));
                command.Parameters.Add(new OracleParameter(":Email", zamestnanec.Email));
                command.Parameters.Add(new OracleParameter(":Heslo", zamestnanec.Heslo));
                command.Parameters.Add(new OracleParameter(":Pozice", zamestnanec.Pozice));
                command.Parameters.Add(new OracleParameter(":Plat", zamestnanec.Plat));
                command.Parameters.Add(new OracleParameter(":DatumNastupu", OracleDbType.Date)
                {
                    Value = zamestnanec.DatumNastupu.ToDateTime(TimeOnly.MinValue)
                });
                command.Parameters.Add(new OracleParameter(":CisloTelefonu", zamestnanec.CisloTelefonu));
                command.Parameters.Add(new OracleParameter(":ZamestnanecZamestnanecId", zamestnanec.ZamestnanecZamestnanecId.HasValue ? (object)zamestnanec.ZamestnanecZamestnanecId.Value : DBNull.Value));
                command.Parameters.Add(new OracleParameter(":AdresaId", zamestnanec.AdresaId));
                command.Parameters.Add(new OracleParameter(":RoleId", zamestnanec.RoleId));
                command.Parameters.Add(new OracleParameter(":SouborId", zamestnanec.SouborId));
                command.Parameters.Add(new OracleParameter(":JePrivate", zamestnanec.JePrivate));

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteAsync(int zamestnanecId)
        {
            var query = "DELETE FROM ZAMESTNANCI WHERE ZAMESTNANEC_ID = :ZamestnanecId";

            using (var connection = _databaseService.GetConnection())
            using (var command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter(":ZamestnanecId", zamestnanecId));

                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
