using BridgeManager.Source.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.Model {
    public abstract class PlayerUnit : IndexedObject {

        public PlayerUnit(int number) : base(number) {
        }
    }
}
