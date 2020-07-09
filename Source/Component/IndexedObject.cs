using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.Component
{
    public abstract class IndexedObject : ObservableObject
    {
        protected int _number;
        protected string _name;

        public int Number {get => _number; set { int old = _number; _number = value; OnPropertyChanged(oldValue: old); } }
        public string Name { get => _name; set { string old = _name; _name = value; OnPropertyChanged(oldValue: old); } }

        protected IndexedObject(int number) {
            this._number = number;
        }
    }
}
