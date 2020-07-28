using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.OleDb;
using BridgeManager.Source.Model;
using System.Data;


namespace BridgeManager.Source.Services.Database {

    public enum BMTable {
        Clients, //ID (set) | Computer (must input exact case-sensitive)
        Session, //ID (unique) | Name (Bridge Game) | Date, Time, GUID (doesnt have to be input) | Status set to 0 | ShowInApp
        Tables, // Section (ID) | Table (ID) | Computer (ID) | Status, LogOnOff, CurrentRound, CurrentBoard and UpdateFromRound ... automatical | Group 
        Section, //ID | Letter | Tables | MissingPair | EWMoveBef (default 0, only when inputing player numbers in "zero round" - players on home table) | Session | Scoring Type = 1 | Winners = 0
        RoundData,
        ReceivedData,
        IntermediateData
    }

    public class AdapterManager {

        private OleDbConnection connection;

        #region SchemaVariables
        private string[] tableNames;
        private string[] tableNamesSafe;
        private string[][] columnNamesSafe;
        private string[][] columnNames;
        private OleDbType[][] columnTypes;
        #endregion Schema

        private OleDbDataAdapter[] adapters;

        public AdapterManager(OleDbConnection connection) {
            SetSchema();
            this.connection = connection;
            this.adapters = new OleDbDataAdapter[7];
        }

        /// <summary>
        /// Returns the name of the given table.
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public string TableName(BMTable table) {
            return tableNames[(int)table];
        }

        /// <summary>
        /// Returns OleDBCommand to remove all entries from a given table
        /// </summary>
        /// <returns></returns>
        public OleDbCommand GetClearCommand(BMTable table) {
            return new OleDbCommand($"DELETE FROM {tableNamesSafe[(int)table]}; ", connection);
        }
        
        /// <summary>
        /// Returns the dataAdapter for the selected table
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public OleDbDataAdapter GetAdapter(BMTable table) {
            if (adapters[(int)table] == null) ConstructAdapter(table);
                return adapters[(int)table];
        }

        private void ConstructAdapter(BMTable table) {
            int t = (int)table;

            //SELECT COMMAND
            var select = $"SELECT * FROM {tableNamesSafe[t]}";

            OleDbDataAdapter adapter = new OleDbDataAdapter(select, connection);

            
            if (table != BMTable.IntermediateData 
                && table != BMTable.ReceivedData) {

                //INSERT COMMAND
                StringBuilder columnsB = new StringBuilder();
                StringBuilder paramsB = new StringBuilder();
                for (int i = 0; i < columnNames[t].Length; i++) {
                    columnsB.Append(columnNamesSafe[t][i] + ",");
                    paramsB.Append("@" + columnNames[t][i] + ",");
                }
                columnsB.Remove(columnsB.Length - 1, 1);
                paramsB.Remove(paramsB.Length - 1, 1);

                var insert = $"INSERT INTO {tableNamesSafe[t]} ({columnsB.ToString()}) VALUES ({paramsB.ToString()});";
                var ins = new OleDbCommand(insert, connection);
                SetParams(t, ins);
                adapter.InsertCommand = ins;
            }

            adapters[(int)table] = adapter;
        }

        /// <summary>
        /// --initialization method
        /// Fills the private atributes
        /// names, columns, columnNames, and columnTypes with values;
        /// </summary>
        private void SetSchema() {
             tableNames = new string[] {
                "Clients", //ID (set) | Computer (must input exact case-sensitive)
                "Session", //ID (unique) | Name (Bridge Game) | Date, Time, GUID (doesnt have to be input) | Status set to 0 | ShowInApp
                "Tables", // Section (ID) | Table (ID) | Computer (ID) | Status, LogOnOff, CurrentRound, CurrentBoard and UpdateFromRound ... automatical | Group 
                "Section", //ID | Letter | Tables | MissingPair | EWMoveBef (default 0, only when inputing player numbers in "zero round" - players on home table) | Session | Scoring Type = 1 | Winners = 0
                "RoundData",
                "ReceivedData",
                "IntermediateData"
            };
            tableNamesSafe = new string[] {
                "Clients", //ID (set) | Computer (must input exact case-sensitive)
                "[Session]", //ID (unique) | Name (Bridge Game) | Date, Time, GUID (doesnt have to be input) | Status set to 0 | ShowInApp
                "Tables", // Section (ID) | Table (ID) | Computer (ID) | Status, LogOnOff, CurrentRound, CurrentBoard and UpdateFromRound ... automatical | Group 
                "[Section]", //ID | Letter | Tables | MissingPair | EWMoveBef (default 0, only when inputing player numbers in "zero round" - players on home table) | Session | Scoring Type = 1 | Winners = 0
                "RoundData",
                "ReceivedData",
                "IntermediateData"
            };
            columnNamesSafe = new string[][] {
                 new string[] {  "ID" , "Computer"},
                 new string[] { "ID", "[Name]", "[Date]", "[Time]", "[GUID]", "Status", "ShowInApp"},
                 new string[] { "[Section]", "[Table]", "ComputerID", "Status", "LogOnOff", "UpdateFromRound", "CurrentRound", "CurrentBoard", "[Group]"},
                 new string[] { "ID", "Letter", "Tables", "MissingPair", "EWMoveBeforePlay", "[Session]", "ScoringType", "Winners"},
                 new string[] { "[Table]", "[Section]", "Round", "NSPair", "EWPair", "LowBoard", "HighBoard", "CustomBoards" },
                 new string[] { },
                 new string[] { }
                };
            columnNames = new string[][]{
                 columnNamesSafe[0],
                 new string[] { "ID", "Name", "Date", "Time", "GUID", "Status", "ShowInApp"},
                 new string[] { "Section", "Table", "ComputerID", "Status", "LogOnOff", "UpdateFromRound", "CurrentRound", "CurrentBoard", "Group"},
                 new string[] {"ID", "Letter", "Tables", "MissingPair", "EWMoveBeforePlay", "Session", "ScoringType", "Winners"},
                 new string[] { "Table", "Section", "Round", "NSPair", "EWPair", "LowBoard", "HighBoard", "CustomBoards" },
                 new string[] { },
                 new string[] { }
                 };
            columnTypes = new OleDbType[][]{
                new OleDbType[] { OleDbType.Integer, OleDbType.VarChar},
                new OleDbType[] { OleDbType.Integer, OleDbType.VarChar, OleDbType.Date, OleDbType.Date, OleDbType.VarChar, OleDbType.Integer, OleDbType.Boolean},
                new OleDbType[] { OleDbType.Integer, OleDbType.Integer, OleDbType.Integer, OleDbType.Integer,  OleDbType.Integer, OleDbType.Integer, OleDbType.Integer, OleDbType.Integer, OleDbType.Integer},
                new OleDbType[] { OleDbType.Integer, OleDbType.VarChar, OleDbType.Integer, OleDbType.Integer,  OleDbType.Integer, OleDbType.Integer, OleDbType.Integer, OleDbType.Integer },
                new OleDbType[] { OleDbType.Integer, OleDbType.Integer, OleDbType.Integer, OleDbType.Integer, OleDbType.Integer, OleDbType.Integer, OleDbType.Integer, OleDbType.VarChar },
                 new OleDbType[] { },
                 new OleDbType[] { }
                 };

        }

        private void SetParams(int table, OleDbCommand cmd) {
            for (int i = 0; i < columnNames[table].Length; i++) {
                var parameter = cmd.Parameters.Add("@" + columnNames[table][i], columnTypes[table][i]);
                parameter.SourceColumn = columnNames[table][i];
                parameter.IsNullable = false;
            }  
        }
        
    }
}
