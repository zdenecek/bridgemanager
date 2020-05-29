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

namespace BridgeManager {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        
        private void Application_Startup(object sender, StartupEventArgs e) {

            Local.Initialize();

            
            //DatabaseHandler db = new DatabaseHandler(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\zdnek\ME\Everything\BridgeManager\BridgeManager\BMInput1.bws");

            MainWindow wnd = new MainWindow();
            wnd.Show();

            Console.SetOut(new ConsoleOutputter(wnd.ConsoleTextBox));
            Console.WriteLine("Welcome to BridgeManager v. 0.1");
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e) {

        }

    }
}
