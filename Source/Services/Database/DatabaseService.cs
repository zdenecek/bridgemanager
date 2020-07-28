using BridgeManager.Source.Services.Database;
using BridgeManager.Source.Model;
using System;
using System.Data;
using System.Data.OleDb;
using System.Linq;


namespace BridgeManager.Source.Services.Database {
    public class DatabaseService : IDatabaseService
    {
        private BMInput1DataSet data;
        private string defaultConnectionString = "Provider = Microsoft.Jet.OLEDB.4.0";
        private OleDbConnection connection;
        private AdapterManager manager;

        public BMInput1DataSet Data { get => data; }

        public DatabaseService()
        {
            data = new BMInput1DataSet();
        }

        public void Open(string filePath)
        {
            Close();
            OleDbConnectionStringBuilder stringBuilder = new OleDbConnectionStringBuilder(this.defaultConnectionString);
            stringBuilder.DataSource = filePath;
            connection = new OleDbConnection(stringBuilder.ConnectionString);
            manager = new AdapterManager(connection);

            try
            {
                Console.WriteLine("Connecting to a database:" + filePath);
                connection.Open();
            }

            catch (Exception e)
            {
                Console.WriteLine("Error opening database file, exception: " + e.Message);
            }
        }

        public void Close()
        {
            connection?.Close();
        }

        public void ClearInDB(BMTable table)
        {
           manager
                .GetClearCommand(table)
                .ExecuteScalar();
        }

        public void FetchFromDB(BMTable table)
        {
            data
                .Tables[manager.TableName(table)].Rows
                .Clear();
            manager
                .GetAdapter(table)
                .Fill(data, manager.TableName(table));
        }
        public void SendToDB(BMTable table)
        {
            manager
                .GetAdapter(table)
                .Update(data, manager.TableName(table));
        }

        public BMInput1DataSet GetData()
        {
            return data;
        }
    }
}
