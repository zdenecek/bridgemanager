using BridgeManager.Source.Component;
using BridgeManager.Source.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BridgeManager.Source.Model
{
    public class Pair : PlayerUnit {

        private Player _player2;
        private Player _player1;
        
        public Player Player1 { get => _player1; set => _player1 = value;   }

        public Player Player2 { get => _player2; set => _player2 = value; }

        // [Persist(Ignore = true)]
        //   public List<Player> Players { get => new List<Player> { Player1, Player2 }; }

        public override string Name { get => Player1.Name + "-" + Player2.Name; }

        public Pair(int number) : base(number) {
        }

        public Pair() : this (0) {
            Player1 = new Player();
            Player2 = new Player();
        }

        /*
        public void ClearReference() {

            this.Player1.Pair = null;
            this.Player2.Pair = null;
        }
        */
        public override string ToString()
        {
            return $"Pair {Number}";
        }

    }
}
