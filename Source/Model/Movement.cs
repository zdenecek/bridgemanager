using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.Model {
    public class Movement {

        private int tableCount, roundCount;
        private RoundTableData[,] data;
        private string description;
        private string name;

        /// <summary>
        /// Contains the data for each round and each table
        /// [roundNumber][tableNumber]
        /// </summary>
        public RoundTableData[,] Data { get => data; set {} }
        public int TableCount { get => tableCount; }
        public int RoundCount { get => roundCount; }
        public string Description { get => description; set => description = value; }
        public string Name { get => name; set => name = value; }

        public Movement(RoundTableData[,] data, String name, String description) {
            this.Name = name;
            this.Data = data;
            this.Description = description;
        }

        public bool ValidateData(RoundTableData[,] data) {
            /*if(data is null || data.Length == 0) return false;
            if (data[0] is null) return false;
            int roundCount = data[0].Length;
            //foreach(RoundTableData[] x in data) { if (x is null || x.Length != roundCount) return false; }
            */        
        return true;

        }
    }

    public class RoundTableData {
        public int pairNS, pairEW, boardsLow, boardsHigh;
    }
}
