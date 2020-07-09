using BridgeManager.Source.Component;
using BridgeManager.Source.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.Model
{
    public class Pair : PlayerUnit {

        private Player _player2;
        private Player _player1;
        

        public Player Player1 { get => _player1; set { _player1 = value;  Player1.Pair = this; } }
        public Player Player2 { get => _player2; set { _player2 = value;  Player2.Pair = this; } }
        

        public string Players() { return Player1.Name + "-" + Player2.Name; }

        public Pair(int number) : base(number) {
        }

        private Pair() : this (0) { }
             
        public void Replace(Player player, Player _new = null) {
            if (player.Equals(Player1)) Player1 = _new;
            if (player.Equals(Player2)) Player2 = _new;
        }

        public void ClearReference() {

            this.Player1.Pair = null;
            this.Player2.Pair = null;
        }

    }
}
