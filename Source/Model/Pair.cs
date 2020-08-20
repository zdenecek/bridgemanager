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
        private string _name;

        public Player Player1 { get => _player1; set => _player1 = value;   }
        public Player Player2 { get => _player2; set => _player2 = value; }

        public override string Name { 
            get => _name ?? AsNames(); 
            set { 
                _name = value; 
                OnPropertyChanged();
            } 
        }

        public bool IsMissing { get; set; }

        public Pair(int number) : base(number) {
            IsMissing = false;
        }

        public Pair() : this (0) {
            //TODO does this have to be here? I think not.
            Player1 = new Player();
            Player2 = new Player();
        }

        public override string ToString()
        {
            return $"Pair {Number}";
        }

        public string AsNames()
        {
            return Player1.Name + "-" + Player2.Name;
        }

    }
}
