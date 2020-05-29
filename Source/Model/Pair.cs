using BridgeManager.Source.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.Model
{
    public class Pair {
        public int Number { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }

        public string Players() { return Player1.Name + "-" + Player2.Name; }

        public Pair(int number) {

            this.Number = number;

        }

    }
}
