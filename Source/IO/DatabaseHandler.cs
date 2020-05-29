using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace BridgeManager.Source.IO
{
    class DatabaseHandler
    {
        private string connectionString;

        private OleDbConnection connection;

        public DatabaseHandler(string connectionStr) {
            this.connectionString = connectionStr;
            this.connection = new OleDbConnection(connectionStr);
            connection.Open();
        }







    }
}
