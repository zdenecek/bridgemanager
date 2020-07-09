using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BridgeManager.Source.ViewModel.Commands {
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"> The Command Parameter type </typeparam>
    public class DelegateCommand<T> : Command {

        private Action<T> execute;
        private Func<T, bool> canBeExecuted;

        public DelegateCommand(Action<T> execute) {
            this.execute = execute;
            this.canBeExecuted = x => true;
        }
        public DelegateCommand(Action<T> execute, Func<T, bool> canExecute) {
            this.execute = execute;
            this.canBeExecuted = canExecute;
        }

        public override bool CanExecute(object parameter) {
            try {
                return canBeExecuted.Invoke((T)parameter);
            }
            catch(Exception e) {
                Console.WriteLine($"Invalid command parameter (CanExecute). Exception: {e.Message} source: {e.Source}");
            }
            return false;
        }
        public override void Execute(object parameter) {
            try {
                Console.WriteLine($"Parameter is null={parameter == null}");
                execute.Invoke((T)parameter);
            }
            catch (Exception e) {
                Console.WriteLine($"Invalid command parameter (Execute). Exception: {e.Message} source: {e.Source}");
               
            }
        }
    }

    public class DelegateCommand : Command {

        private Action execute;
        private Func<bool> canBeExecuted;

        public DelegateCommand(Action execute) {
            this.execute = execute;
            this.canBeExecuted = () => true;
        }
        public DelegateCommand(Action execute, Func<bool> canExecute) {
            this.execute = execute;
            this.canBeExecuted = canExecute;
        }

        public override bool CanExecute(object parameter) {
            return canBeExecuted.Invoke();        
        }
        public override void Execute(object parameter) {
            execute.Invoke();
        }
    }
}
