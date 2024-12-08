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

        public OracleConnection GetConnection()
        {
            var connection = new OracleConnection(_configuration.GetConnectionString("DefaultConnection"));
            connection.Open();
            return connection;
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                if (_connection.State != ConnectionState.Closed)
                {
                    _connection.Close();
                }
                _connection.Dispose();
                _connection = null;
            }
        }
    }
}