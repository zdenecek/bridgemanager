using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BridgeManager.Source.ViewModel.Commands {
    public abstract class Command : ICommand {
        public event EventHandler CanExecuteChanged;

        public void OnCanExecuteChanged(object sender, EventArgs args) {
            this.CanExecuteChanged?.Invoke(sender, args);  
        }

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);
    }
}
