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
using Autofac;
using System.Reflection;
using BridgeManager.Source.Model;
using BridgeManager.Source.Services;
using BridgeManager.Source.ViewModel.Commands;

namespace BridgeManager {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        
        

        private void Application_Startup(object sender, StartupEventArgs e) {

            var container = ContainerConfig.Configure();

            var scope = container.BeginLifetimeScope();
            ConfigureMainWindowViewmodel(scope);

            var mainViewModel = scope.Resolve<MainWindowViewModel>();
            

            Console.SetOut(new ConsoleOutputter(mainViewModel.MainWindow.ConsoleTextBox));
            Console.WriteLine("Welcome to BridgeManager v. 0.1");

            Strings.Initialize();

            defaultTest(scope);
        }

        private void defaultTest(ILifetimeScope scope)
        {
            scope.Resolve<TournamentViewModel>().Open(@"c:\Users\zdnek\ME\Bridge\BridgeManager\testfiles\tx.json");
            scope.Resolve<ScoringViewModel>().CreateScores();
        }

        private void ConfigureMainWindowViewmodel(ILifetimeScope scope)
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
