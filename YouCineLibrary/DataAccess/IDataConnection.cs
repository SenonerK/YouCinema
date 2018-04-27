using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouCineLibrary.DataAccess
{
    public interface IDataConnection
    {
        bool TestConnection();
    }
}
