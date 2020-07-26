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
        public Player(int number) : base(number) {
        }

        public Player() : this(0) { }

    }
}
