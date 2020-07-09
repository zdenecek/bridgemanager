using BridgeManager.Source.Model;
using System;
using System.Data;
using System.Data.OleDb;
using System.Linq;


namespace BridgeManager.Source.IO.Database {
    public class DatabaseService : IDisposable {


        private string defaultConnectionString = "Provider = Microsoft.Jet.OLEDB.4.0";
        private OleDbConnection connection;
        private AdapterManager manager;
        private BMInput1DataSet data;

        public void Dispose() {
            connection?.Close();
        }
        public void SendMovementData(Session session) {
            Open(session.DatabaseFilepath);

            //CLIENTS
            var client = CheckClientsAndGetNumber();

            //SESSIONS
            ClearInDB(BMTable.Session);
            {
                var r1 = data.Session.NewSessionRow();
                r1.ID = 1;
                r1.Name = session.Name;
                r1.Date = DateTime.Today;
                r1.Time = DateTime.Now;
                r1.ShowInApp = true;
                r1.GUID = String.Empty;
                r1.Status = 0;
                data.Session.AddSessionRow(r1);
            }

            

            //SECTIONS TABLES AND ROUNDDATA
            ClearInDB(BMTable.Section);
            ClearInDB(BMTable.Tables);
            ClearInDB(BMTable.RoundData);
            {
                foreach (Section s in session.Sections) {
                    //SECTION

                    var r2 = data.Section.NewSectionRow();
                    r2.ID = (short)s.Number;
                    r2.Letter = s.Name;
                    r2.Tables = (short)s.Movement.TableCount;
                    r2.MissingPair = 0;
                    r2.Session = 1;

                    r2.EWMoveBeforePlay = 0;
                    r2.ScoringType = 1;
                    r2.Winners = 0;
                    data.Section.AddSectionRow(r2);

                    //TABLES
                    ClearInDB(BMTable.Tables);
                    for (int i = 1; i <= s.Movement.TableCount; i++) {
                        var r3 = data._Tables.NewTablesRow();
                        r3.Section = r2.ID;
                        r3._Table = (short)i;
                        r3.ComputerID = (short)client;
                        r3.Status = 0;
                        r3.LogOnOff = 2;
                        r3.UpdateFromRound = 0;
                        r3.CurrentBoard = 0;
                        r3.CurrentRound = 0;
                        r3.Group = 0;
                        data._Tables.AddTablesRow(r3);

                    }

                    //ROUND DATA
                    for (int t = 0; t < s.Movement.Data.GetLength(0); t++) {
                        for (int r = 0; r < s.Movement.Data.GetLength(1); r++) {
                            RoundTableData info = s.Movement.Data[t, r];
                            var r4 = this.data.RoundData.NewRoundDataRow();
                            r4.Section = r2.ID;
                            r4._Table = (Int16)info.table;
                            r4.Round = (Int16)info.round;
                            r4.NSPair = (Int16)info.pairNS;
                            r4.EWPair = (Int16)info.pairEW;
                            if (info.UsesCustomBoards) {
                                r4.CustomBoards = info.customBoards;
                                r4.LowBoard = 1;
                                r4.HighBoard = 2;
                            }
                            else {
                                r4.LowBoard = (Int16)(info.boardsLow + s.BoardsShift);
                                r4.HighBoard = (Int16)(info.boardsHigh + s.BoardsShift);
                                r4.CustomBoards = String.Empty;
                            }
                            data.RoundData.AddRoundDataRow(r4);
                        }
                    }
                }
            }


            Send(BMTable.Session);
            Send(BMTable.Section);
            Send(BMTable.Tables);
            Send(BMTable.RoundData);
        }

        public void RetrieveResults(Session session, Tournament tournament) {
            Open(session.DatabaseFilepath);

            Fetch(BMTable.ReceivedData);
           // Fetch(BMTable.IntermediateData);

            var sections = session.Sections.ToLookup<Section, int>(s => s.Number);
            
            var pairs = tournament.Pairs.ToLookup<Pair, int>(p => p.Number);

            session.Results.Clear();

            foreach(BMInput1DataSet.ReceivedDataRow r in data.ReceivedData.Rows) {
                Console.WriteLine("Still going, timelog="+r.TimeLog);
                session.Results.Add(new Result() {
                    Section = sections[r.Section].First(),
                    Table = r._Table,
                    Round = r.Round,
                    Board = r.Board,
                    PairNS = pairs[r.PairNS].First(),
                    PairEW = pairs[r.PairEW].First(),
                    Declarer = pairs[r.Declarer].First(),
                    NSEW = r._NS_EW,
                    Contract = r.Contract,
                    _Result = r.Result,
                    LeadCard = r.LeadCard,
                    Notes = r.Remarks,
                    Erased = r.Erased    
                });
            }
            Console.WriteLine("Results retrieved sucessfully!");
        }

        private void Open(string filePath) {
            connection?.Close();
            OleDbConnectionStringBuilder stringBuilder = new OleDbConnectionStringBuilder(this.defaultConnectionString);
            stringBuilder.DataSource = filePath;
            connection = new OleDbConnection(stringBuilder.ConnectionString);
            manager = new AdapterManager(connection);
            data = new BMInput1DataSet();

            try {
                Console.WriteLine("Connecting to a database:" + filePath);
                connection.Open();
            }
            catch (Exception e) {
                Console.WriteLine("Error opening database file, exception: " + e.Message);
            }
        }

        private int CheckClientsAndGetNumber() {
            Fetch(BMTable.Clients);
            var name = Environment.MachineName;
            foreach (BMInput1DataSet.ClientsRow row in data.Clients.Rows) {
                if (row.Computer.Equals(name)) return row.ID;
            }
            var _row = data.Clients.NewClientsRow();
            _row.ID = data.Clients.Rows.Count + 1;
            _row.Computer = name;
            data.Clients.AddClientsRow(_row);
            Send(BMTable.Clients);
            return _row.ID;
        }

        #region ElementaryDataOperations

        private void ClearInDB(BMTable table) {
            var x = manager.GetClearCommand(table);

            manager
                .GetClearCommand(table)
                .ExecuteScalar();
        }
        private void Fetch(BMTable table) {
            data.Tables[manager.TableName(table)].Rows.Clear();
            manager
                .GetAdapter(table)
                .Fill(data, manager.TableName(table));
        }
        private void Send(BMTable table) {
            manager
                .GetAdapter(table)
                .Update(data, manager.TableName(table));
        }

        #endregion

    }
}
