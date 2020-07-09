using BridgeManager.Source.Model;
using BridgeManager.Source.ViewModel.Commands;
using BridgeManager.Source.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BridgeManager.Source.ViewModel {
    public class MovementSelectorWindowViewModel {

        public IEnumerable<Movement> Movements { get; private set; }
        public MovementSelectorWindow MovementSelectorWindow { get; private set; }
        public DelegateCommand OKCommand { get; private set; } 

        public MovementSelectorWindowViewModel(IEnumerable<Movement> movements) {
            MovementSelectorWindow = new MovementSelectorWindow();

            OKCommand = new DelegateCommand(OK_Click);

            this.Movements = movements;

            MovementSelectorWindow.DataContext = this;            
        }

        public void OK_Click() {
            MovementSelectorWindow.Close();
        }

    }
}
