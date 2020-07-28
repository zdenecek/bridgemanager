using BridgeManager.Source.Component;
using BridgeManager.Source.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BridgeManager.Source.ViewModel
{
    public abstract class ViewModelBase : ObservableObject {

        private readonly MainWindowViewModel _mainViewModel;
        protected ContentControl _view;
        private String header;

        public MainWindowViewModel MainViewModel { get => _mainViewModel; }
        public ContentControl View { get => _view; }
        public string Header { get => header; set { header = value; OnPropertyChanged(); } }

        protected bool IsLoaded { get => MainViewModel.IsLoaded; }

        protected ViewModelBase(MainWindowViewModel windowViewModel) {
            this._mainViewModel = windowViewModel;
        }

      
           
    }
}
