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

namespace BridgeManager.Source.ViewModel
{
    public class BridgemateViewModel : ViewModelBase
    {
        public string BCSFileName = @"C:\Program Files (x86)\Bridgemate Pro\BMPro.exe";
    
        public string bcsArgs = "/f:[{database}] /s /r";

        private DatabaseService databaseService;

        public Command SendCurrentSessionCommand { get; private set; }
        public Command StartBCSCommand { get; private set; }

        public Command RetrieveResultsCommand { get; private set; }

        public BridgemateViewModel(MainWindowViewModel mainController) : base(mainController) {
            Header = "BM";
            _view = new BridgemateControl();
            databaseService = new DatabaseService();

            this.SendCurrentSessionCommand = new DelegateCommand(SendDataToBM);
            this.StartBCSCommand = new DelegateCommand(StartBCS);
            this.RetrieveResultsCommand = new DelegateCommand(RetrieveResults);
        }

        public void SendDataToBM() {
            //Add checks
            if(MainViewModel.LoadedSession.DatabaseFilepath is null) {
                Console.WriteLine("No filepath is set for current session");
                return;
            }
            databaseService.SendMovementData(MainViewModel.LoadedSession);
            Console.WriteLine("Database successfully updated!");
        }

        public void StartBCS() {

            var database = MainViewModel.LoadedSession.DatabaseFilepath;

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = BCSFileName,
                Arguments = bcsArgs.Replace("{database}", database)
            };

            var process = new Process() { StartInfo = startInfo };

            process.Start();
        }

        public void RetrieveResults() {
            databaseService.RetrieveResults(MainViewModel.LoadedSession, MainViewModel.LoadedTournament) ;
        }

    }
}
