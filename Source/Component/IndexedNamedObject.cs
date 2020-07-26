using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.Component
{
    public delegate void IndexChangedEventHandler(IndexedNamedObject sender, int oldIndex, int newIndex);

    public abstract class IndexedNamedObject : ObservableObject
    {
        public event IndexChangedEventHandler IndexChanged;

        private int _number;
        private string _name;

        public  int Number {
            get => _number; 
            set { 
                int old = _number; 
                _number = value; 
                OnPropertyChanged();
                OnIndexChanged(old, value);
            } 
        }
        
        public virtual string Name { 
            get => _name; 
            set 
            {   _name = value; 
                OnPropertyChanged(); 
            } 
        }

        protected IndexedNamedObject(int number) {
            this._number = number;
        }

        protected void OnIndexChanged(int oldIndex, int newIndex)
        {
            IndexChanged?.Invoke(this, oldIndex, newIndex);
        }
    }
}
