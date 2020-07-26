using BridgeManager.Source.Component;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BridgeManager.Source.Model {
    public class Movement : IndexedNamedObject {

        private int _tableCount, _roundCount;

        private string description;

        /// <summary>
        /// Contains the data for each round and each table
        /// [TABLE, ROUND]
        /// </summary>
        /// 
        [XmlIgnore]
        public RoundTableData[,] Data { get; set; }

      
        [XmlElement("Data"), Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public List<RoundTableData> XMLData
        {
            get {
                var x = new List<RoundTableData>();
                if (Data == null) return x;

                foreach (RoundTableData t in Data)
                { x.Add(t); }
                return x;
                /*
                RoundTableData[][] x = new RoundTableData[Data.GetLength(0)][];
                for (int i = 0; i < Data.GetLength(0); i++)
                {
                    x[i] = new RoundTableData[Data.GetLength(1)];
                    for (int b = 0; b < Data.GetLength(1); b++)
                    {
                        x[i][b] = Data[i, b];
                    }
                }
                return x;
                */
            }
            set { 
                int tables = value.Select(d => d.table).Max();
                var data = new RoundTableData[tables, value.Count/tables];
                foreach(var d in value)
                {
                    data[d.table, d.round] = d;
                }
                this.Data = data;
            }
        }
        public int TableCount { get => _tableCount; }
        public int RoundCount { get => _roundCount; }
        public string Description { get => description; set => description = value; }

        public Movement(int number, RoundTableData[,] data, string name, string description) : base(number) {
            this.Name = name;
            this.Data = data;
            this.Description = description;
            this._roundCount = data.GetLength(1);
            this._tableCount = data.GetLength(0);
        }

        public Movement() : base(0) { }

        public bool ValidateData(RoundTableData[,] data) {
            /*if(data is null || data.Length == 0) return false;
            if (data[0] is null) return false;
            int roundCount = data[0].Length;
            //foreach(RoundTableData[] x in data) { if (x is null || x.Length != roundCount) return false; }
            */
            return true;
        }
    }

    public class RoundTableData
    {
        public int pairNS;
        public int pairEW;
        public int boardsLow;
        public int boardsHigh;
        public string customBoards;
        public int table;
        public int round;

        public bool UsesCustomBoards { get => customBoards.Equals(string.Empty); }


    }
}

    
