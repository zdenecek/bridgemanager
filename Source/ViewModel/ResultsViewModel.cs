using BridgeManager.Source.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using BridgeManager.Source.ViewModel.Commands;
using BridgeManager.Source.Views;
using BridgeManager.Source.IO.Database;
using System.Diagnostics;
using BridgeManager.Source.Services.Database;
using BridgeManager.Source.Services;
using BridgeManager.Source.IO;
using System.Collections.ObjectModel;
using BridgeManager.Source.Model.Scoring;

namespace BridgeManager.Source.ViewModel
{
    public class ResultsViewModel : ViewModelBase
    {

        public ObservableCollection<Result> Results { get => MainViewModel.LoadedSession?.Results; }


        public Command Command { get; private set; }


        public ResultsViewModel(MainWindowViewModel mainController) : base(mainController) {
            Header = "Results";
            _view = new ResultsControl();

           
        }

      

    }
}
