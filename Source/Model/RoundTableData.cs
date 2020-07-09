using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.Model {
    public class RoundTableData {
        public int table, round, pairNS, pairEW, boardsLow, boardsHigh;
        public string customBoards;

        public List<int> boards() {
            List<int> ret = new List<int>();

            if (UsesCustomBoards) {
                foreach (string b in customBoards.Split(',')) {
                    ret.Add(Int32.Parse(b.Trim()));
                }
            }
            else {
                for (int i = boardsLow; i <= boardsHigh; i++) {
                    ret.Add(i);
                }
            }
            return ret;
        }

        public bool UsesCustomBoards { get => customBoards == string.Empty; }
    }
}

