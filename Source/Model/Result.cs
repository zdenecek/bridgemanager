using BridgeManager.Source.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.Model {
    public class Result {
        
        public Section Section { get; set; }
        public int Table { get; set; }
        public int Round { get; set; }
        public int Board { get; set; }
        public Pair PairNS { get; set; }
        public Pair PairEW { get; set; }

        public Pair Declarer { get; set; }

        public string NSEW { get; set; }

        public string Contract { get; set; }
        public string _Result { get; set; }
        public string LeadCard { get; set; }

        public string Notes { get; set; }

        public bool Erased { get; set; }

        public bool Processed { get; set; } = false;

        public int NSPoints() {
            return BridgeMath.GetNSScore(this);
        }

        public int EWPoints() {
            return -NSPoints();
        }

        public bool NotPlayedOrArbitraryScore() {
            return Contract == "";
        }

        
    }
}
