using BridgeManager.Source.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.Model
{
    public class Player : PlayerUnit
    {  
        private Pair _pair = null;

        public Pair Pair { get => _pair; set { _pair = value; OnPropertyChanged(); } }

        public Player(int number) : base(number) {
        }

        private Player() : this(0) { }

    }
}
