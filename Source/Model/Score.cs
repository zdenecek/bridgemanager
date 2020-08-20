using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.Model.Scoring {
    public class Score{

        public static Score operator +(Score a, Score b)
        {
            if (a == null) return b;
            if (b == null) return a;

            return new Score()
            {
                PlayerUnit = a.PlayerUnit,
                OpponentPlayerUnit = a.OpponentPlayerUnit,
                MaxPoints = a.MaxPoints + b.MaxPoints,
                Points = a.Points + b.Points
            };
        }

        public Result AsociatedResult { get; set; }

        public PlayerUnit PlayerUnit { get; set; }

        public PlayerUnit OpponentPlayerUnit { get; set; }
        
        public decimal Points { get; set; }

        public decimal MaxPoints { get; set; }

        [JsonIgnore]
        public string Percentage { get => $"{ Math.Round(Points / MaxPoints * 100, 3) }%"; }

    }
}
