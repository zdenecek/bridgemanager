using BridgeManager.Source.Model;
using BridgeManager.Source.ViewModel.Commands;
using BridgeManager.Source.Views;
using System.Collections.ObjectModel;

namespace BridgeManager.Source.ViewModel
{
    public class ResultsViewModel : ViewModelBase
    {

        public ObservableCollection<Result> Results { get => MainViewModel.LoadedSession?.Results; }


        public Command Command { get; private set; }

        public override string Header { get => Properties.Strings.results_title; }


        public ResultsViewModel(MainWindowViewModel mainController) : base(mainController) {
           
            _view = new ResultsControl();

           
        }

      

    }
}
