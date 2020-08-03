using BridgeManager.Source.ViewModel.Commands;
using BridgeManager.Source.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.ViewModel
{
    public class ExportsViewModel : ViewModelBase
    {
        public override string Header => Properties.Strings.exports_title;

        public Command c;

        public ExportsViewModel(MainWindowViewModel mainWindowViewModel) : base(mainWindowViewModel)
        {
            this._view = new ExportsControl();


        }
    }
}
