using BridgeManager.Source.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.Model {
    public class Result {

        public Result()
        {
        }

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

        public bool NotPlayedOrArbitraryScore
        {
            get => Contract == "";
        }

        public string Short { get => $"{Contract} {NSEW} {_Result}"; }


        public int NSPoints() {
            if (NotPlayedOrArbitraryScore) throw new Exception("Result is 'not played', cannot get points");
             return BridgeMath.GetNSScore(this);
           
        }

        public int EWPoints() {
            if (NotPlayedOrArbitraryScore) throw new Exception("Result is 'not played', cannot get points");
            return -NSPoints();
           
        }




        public override string ToString()
        {
            return $" { (Erased? "ERASED" : "") } Section {Section.Name} Table {Table} Round {Round} Board {Board} NS: ({PairNS.Number}) EW: ({PairEW.Number})" +
                $" {Contract} {NSEW} {_Result} Lead {LeadCard} " +
                $"Notes: {Notes}";
        }


    }
}
