using System;
using System.Data;
using System.Linq;
using Npgsql;

namespace YouCineLibrary.DataAccess
{
    public class PSqlConnector : IDataConnection
    {
        public string ConnectionString { get; set; }

        public PSqlConnector(string CnnString)
        {
            ConnectionString = CnnString;
        }

        private bool hasConnectionString()
        {
            if (!string.IsNullOrEmpty(ConnectionString) && !string.IsNullOrWhiteSpace(ConnectionString))
                return true;

            return false;
        }

        public bool TestConnection()
        {
            try
            {
                if (!hasConnectionString())
                    return false;

                using (NpgsqlConnection cnn = new NpgsqlConnection(ConnectionString))
                {
                    cnn.Open();
                    cnn.Close();
                    return true;
                }
            }
            catch { return false; }
        }
    }
}
