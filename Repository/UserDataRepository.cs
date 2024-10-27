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

//namespace DopravniPodnikSem.Repository
//{
//    public class UserDataRepository : IUserDataRepository
//    {
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
//    }
//}
