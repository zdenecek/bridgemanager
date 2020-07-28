using Autofac;
using BridgeManager.Source.Services;
using BridgeManager.Source.Utilities;
using BridgeManager.Source.ViewModel;
using System;
using System.Threading;
using System.Windows;

namespace BridgeManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        private MainWindowViewModel mainWindowViewModel;


        public static void ChangeCulture(string culture)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(culture);
        }

        private void Application_Startup(object sender, StartupEventArgs e) {

            ChangeCulture(BridgeManager.Properties.Settings.Default.ui_language);
            BridgeManager.Properties.Strings.Culture = new System.Globalization.CultureInfo(BridgeManager.Properties.Settings.Default.ui_language);

             var container = ContainerConfig.Configure();

            var scope = container.BeginLifetimeScope();
            ConfigureNestedViewModels(scope);

            this.mainWindowViewModel = scope.Resolve<MainWindowViewModel>();

            ConfigureMainWindow();

            //----------------------------------------------------------
                        
            Console.WriteLine("Welcome to BridgeManager v. 0.1");
            Console.WriteLine("Current language is:" + Thread.CurrentThread.CurrentUICulture);
            Console.WriteLine(BridgeManager.Properties.Strings.bridgemate_retrieve_results);

            defaultTest(scope);
        }

        private void ConfigureMainWindow()
        {
            MainWindow = mainWindowViewModel.MainWindow;
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            
            Console.SetOut(new ConsoleOutputter(mainWindowViewModel.MainWindow.ConsoleTextBox));
        }

        private void defaultTest(ILifetimeScope scope)
        {
            scope.Resolve<TournamentViewModel>().Open(@"c:\Users\zdnek\ME\Bridge\BridgeManager\testfiles\tx.json");
            scope.Resolve<ScoringViewModel>().CreateScores();
            Console.WriteLine(Thread.CurrentThread.CurrentCulture.ToString() + Thread.CurrentThread.CurrentUICulture.ToString());
        }

        private void ConfigureNestedViewModels(ILifetimeScope scope)
        {
            var mainController = scope.Resolve<MainWindowViewModel>();

            mainController.ViewModels.Add(scope.Resolve<TournamentViewModel>());
            mainController.ViewModels.Add(scope.Resolve<SessionsViewModel>());
            mainController.ViewModels.Add(scope.Resolve<SectionsViewModel>());
            mainController.ViewModels.Add(scope.Resolve<MovementsViewModel>());
            mainController.ViewModels.Add(scope.Resolve<PlayersViewModel>());
            mainController.ViewModels.Add(scope.Resolve<BridgemateViewModel>());
            mainController.ViewModels.Add(scope.Resolve<ScoringViewModel>());
            mainController.ViewModels.Add(scope.Resolve<ResultsViewModel>());

            return;
        }


    }
}
