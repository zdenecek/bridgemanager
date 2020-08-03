using Autofac;
using BridgeManager.Source.Cultures;
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


        private void Application_Startup(object sender, StartupEventArgs e) {

            CultureResources.ChangeCulture(BridgeManager.Properties.Settings.Default.ui_language);
          
            var container = ContainerConfig.Configure();

            var scope = container.BeginLifetimeScope();
            ConfigureNestedViewModels(scope);

            this.mainWindowViewModel = scope.Resolve<MainWindowViewModel>();

            ConfigureMainWindow();

            //----------------------------------------------------------
                        
            Console.WriteLine("Welcome to BridgeManager v. 0.1");
            
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
            mainController.ViewModels.Add(scope.Resolve<ExportsViewModel>());
            return;
        }


    }
}
