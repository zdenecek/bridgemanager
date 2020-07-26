using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.Model.Scoring {
    public class Score{

        public Result AsociatedResult { get; set; }

        public PlayerUnit PlayerUnit { get; set; }

        public PlayerUnit OpponentPlayerUnit { get; set; }
        
        public decimal Points { get; set; }

        public decimal MaxPoints { get; set; }

        [JsonIgnore]
        public string Percentage { get => $"{ Math.Round(Points / MaxPoints * 100, 3) }%"; }

    }
}
