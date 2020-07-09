using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.Model.Scoring {
    public class PartialScore : Score {

        public Result AsociatedResult { get; set; }

        public PlayerUnit OpponentPlayerUnit { get; set; }
        
        public int Points { get; set; }

        public int MaxPoints { get; set; }

    }
}
