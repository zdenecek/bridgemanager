using BridgeManager.Source.Services.Database;
using BridgeManager.Source.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace BridgeManager.Source.Services.Database
{
    public class BCSService : IBCSService
    {
        private IDatabaseService db;

        public BCSService(IDatabaseService databaseService)
        {
            db = databaseService;
        }

        public void SendMovementData(Session session)
        {
            db.Open(session.DatabaseFilepath);

            var clientNumber = CheckClientsAndGetNumber();

            db.ClearInDB(BMTable.Session);
            db.ClearInDB(BMTable.Section);
            db.ClearInDB(BMTable.Tables);
            db.ClearInDB(BMTable.RoundData);

            PopulateSessionTable(session);
            PopulateSectionsAndRoundDataAnd_TablesTables(session, clientNumber);

            db.SendToDB(BMTable.Session);
            db.SendToDB(BMTable.Section);
            db.SendToDB(BMTable.Tables);
            db.SendToDB(BMTable.RoundData);

            db.Close();


            void PopulateSessionTable(Session _session)
            {
                var r1 = db.Data.Session.NewSessionRow();
                r1.ID = 1;
                r1.Name = _session.Name;
                r1.Date = DateTime.Today;
                r1.Time = DateTime.Now;
                r1.ShowInApp = true;
                r1.GUID = String.Empty;
                r1.Status = 0;
                db.Data.Session.AddSessionRow(r1);
            }

            BMInput1DataSet.SectionRow AddSectionRow(Section s)
            {
                var row = db.Data.Section.NewSectionRow();
                row.ID = (short)s.Number;
                row.Letter = s.Name;
                row.Tables = (short)s.Movement.TableCount;
                row.MissingPair = 0;
                row.Session = 1;

                row.EWMoveBeforePlay = 0;
                row.ScoringType = 1;
                row.Winners = 0;
                db.Data.Section.AddSectionRow(row);
                return row;
            }

            void PopulateSectionsAndRoundDataAnd_TablesTables(Session __session, int __clientNumber)
            {
                foreach (Section section in __session.Sections)
                {
                    var sectionRow = AddSectionRow(section);

                    for (int i = 1; i <= section.Movement.TableCount; i++)
                    {
                        AddTablesRow(__clientNumber, sectionRow.ID, i);
                    }

                    for (int t = 0; t < section.Movement.Data.GetLength(0); t++)
                    {
                        for (int r = 0; r < section.Movement.Data.GetLength(1); r++)
                        {
                            AddRoundDataRow(section, sectionRow.ID, t, r);
                        }
                    }
                }

                void AddTablesRow(int _clientNumber, short sectionIDinDB, int table)
                {
                    var r3 = db.Data._Tables.NewTablesRow();
                    r3.Section = sectionIDinDB;
                    r3._Table = (short)table;
                    r3.ComputerID = (short)_clientNumber;
                    r3.Status = 0;
                    r3.LogOnOff = 2;
                    r3.UpdateFromRound = 0;
                    r3.CurrentBoard = 0;
                    r3.CurrentRound = 0;
                    r3.Group = 0;
                    db.Data._Tables.AddTablesRow(r3);
                }

                void AddRoundDataRow(Section section, short sectionIDinDB, int table, int round)
                {
                    RoundTableData info = section.Movement.Data[table, round];
                    var r4 = db.Data.RoundData.NewRoundDataRow();
                    r4.Section = sectionIDinDB;
                    r4._Table = (Int16)info.table;
                    r4.Round = (Int16)info.round;
                    r4.NSPair = (Int16)info.pairNS;
                    r4.EWPair = (Int16)info.pairEW;
                    if (info.UsesCustomBoards)
                    {
                        r4.CustomBoards = info.customBoards;
                        r4.LowBoard = 1;
                        r4.HighBoard = 2;
                    }
                    else
                    {
                        r4.LowBoard = (Int16)(info.boardsLow + section.BoardsShift);
                        r4.HighBoard = (Int16)(info.boardsHigh + section.BoardsShift);
                        r4.CustomBoards = String.Empty;
                    }
                    db.Data.RoundData.AddRoundDataRow(r4);
                }
            }
        }

        public void RetrieveResults(Session session, Tournament tournament)
        {
            try
            {
                db.Open(session.DatabaseFilepath);
                db.FetchFromDB(BMTable.ReceivedData);
                CheckDBConsistentWithModel(session, tournament);
                var sections = session.Sections.ToLookup<Section, int>(s => s.Number);
                var pairs = tournament.Pairs.ToLookup<Pair, int>(p => p.Number);

                session.Results.Clear();

                foreach (BMInput1DataSet.ReceivedDataRow r in db.Data.ReceivedData.Rows)
                {
                    session.Results.Add(new Result()
                    {
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
            }
            catch (InconsistentModelException)
            {
                throw;
            }
            catch (OleDbException)
            {
                throw;
            }
            finally
            {
                db.Close();
            }
        }

        private void CheckDBConsistentWithModel(Session session, Tournament tournament)
        {
            try
            {
                var pairsInDB = db.Data.ReceivedData.Max(r => Math.Max(r.PairEW, r.PairNS));
                var pairsInModel = tournament.Pairs.Count;

                check(pairsInModel, pairsInDB, InconsistentModelException.ModelDBInconsistencyType.Pairs);

                var sectionsInDB = db.Data.ReceivedData.Max(r => r.Section);
                var sectionsInModel = session.Sections.Count;

                check(sectionsInModel, sectionsInDB, InconsistentModelException.ModelDBInconsistencyType.Sections);
            }
            catch(InconsistentModelException)
            {
                throw;
            }
            
            void check(int model, int db, InconsistentModelException.ModelDBInconsistencyType type)
            {
                if (db > model)
                    throw new InconsistentModelException(
                        InconsistentModelException.ModelDBInconsistencyType.Sections,
                        db,
                        model);
            }
        }

        public int CheckClientsAndGetNumber()
        {
            db.FetchFromDB(BMTable.Clients);
            var name = Environment.MachineName;
            foreach (BMInput1DataSet.ClientsRow row in db.Data.Clients.Rows)
            {
                if (row.Computer.Equals(name)) return row.ID;
            }
            var _row = db.Data.Clients.NewClientsRow();
            _row.ID = db.Data.Clients.Rows.Count + 1;
            _row.Computer = name;
            db.Data.Clients.AddClientsRow(_row);
            db.SendToDB(BMTable.Clients);
            return _row.ID;
        }
    }

    internal class InconsistentModelException : Exception
    {
        public enum ModelDBInconsistencyType { Pairs, Sections};

        public ModelDBInconsistencyType InconsistencyType { get; private set; }
        public int CountInDB { get; private set; }
        public int CountInModel { get; private set; }

        public InconsistentModelException()
        {
        }

        public InconsistentModelException(ModelDBInconsistencyType type, int countInDB, int countInModel)
        {
            this.InconsistencyType = type;
            this.CountInDB = countInDB;
            this.CountInModel = countInModel;
        }

    }
}
