using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.Model
{
    public class Section
    {
        private int number;
        private char name;
        private Dictionary<int, int> playerMatrix;
        private Movement movement;

        public char Name { get => name; set => name = value; }
        public Movement Movement { get => movement; set => movement = value; }     
        public Dictionary<int, int> PlayerMatrix { get => playerMatrix; set => playerMatrix = value; }
        public int Number { get => number; set => number = value; }

        public Section() {
            this.playerMatrix = new Dictionary<int, int>();
            this.Number = 0;
            this.name = 'A';
        }
    }
}
