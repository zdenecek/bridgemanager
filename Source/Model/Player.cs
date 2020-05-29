using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.Model
{
    public class Player
    {
        private int number;
        private string name;

        public int Number { get => number; set => number = value; }
        public string Name { get => name; set => name = value; }

        public Player(int number) {
            this.number = number;
        }

    }
}
