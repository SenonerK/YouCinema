using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouCineLibrary.DataAccess;

namespace YouCineLibrary
{
    public static class Config
    {
        public static IDataConnection Connection { get; private set; }

        public static void InitializeConnection(ConnectionType type, string CnnString)
        {
            switch (type)
            {
                case ConnectionType.PostgreSQL:
                    Connection = new PSqlConnector(CnnString);
                    break;
                case ConnectionType.MySQL:
                    // ...
                    break;
            }
        }

        public static void RemoveConnection()
        {
            Connection = null;
        }
    }
}
