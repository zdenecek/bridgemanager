using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.ViewModel
{
    public abstract class DelegateController
    {
        private MainController mainController;

        public MainController MainController { get => mainController; }

        protected DelegateController(MainController mainController) {
            this.mainController = mainController;
        }

        
    }
}
