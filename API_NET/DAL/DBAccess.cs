using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_NET.DAL
{
    public class DBAccess
    {
        private string connectionString;

        public string GetConnectionString{ get => connectionString;}

        public DBAccess(string ConnectionString)
        {
            this.connectionString = ConnectionString;
        }
    }
}
