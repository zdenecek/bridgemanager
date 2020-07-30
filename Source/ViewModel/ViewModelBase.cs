using BridgeManager.Source.Component;
using BridgeManager.Source.Cultures;
using BridgeManager.Source.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace BridgeManager.Source.ViewModel
{
    public abstract class ViewModelBase : ObservableObject {

        private readonly MainWindowViewModel _mainViewModel;
        protected ContentControl _view;

        public MainWindowViewModel MainViewModel { get => _mainViewModel; }
        public ContentControl View { get => _view; }
        public abstract string Header { get; }
            
        protected bool IsLoaded { get => MainViewModel.IsLoaded; }

        protected ViewModelBase(MainWindowViewModel windowViewModel) {
            this._mainViewModel = windowViewModel;

            CultureResources.CultureChanged += (s, e) => OnPropertyChanged("Header");
        }

      
           
    }
}
