using BridgeManager.Source;
using BridgeManager.Source.ViewModel;
using BridgeManager.Source.IO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BridgeManager.Source.Tools;

namespace BridgeManager {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        
        private void Application_Startup(object sender, StartupEventArgs e) {

            var container = ContainerConfig.Configure();

            using(var scope = container.BeginLifetimeScope())
            {

            }
      
            MainWindowViewModel mainController = new MainWindowViewModel();

        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e) {

        }

    }
}
