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
using BridgeManager.Source.Services.Database;
using BridgeManager.Source.Services;

namespace BridgeManager.Source.ViewModel
{
    public class BridgemateViewModel : ViewModelBase
    {
        public string BCSFileName = @"C:\Program Files (x86)\Bridgemate Pro\BMPro.exe";
    
        public string bcsArgs = "/f:[{database}] /s /r";

        private IBCSService bcsService;
        private IDialogService dialogService;

        public Command SendCurrentSessionCommand { get; private set; }
        public Command StartBCSCommand { get; private set; }

        public Command RetrieveResultsCommand { get; private set; }

        public BridgemateViewModel(MainWindowViewModel mainController, IBCSService bcs, IDialogService dialogService) : base(mainController) {
            Header = "BM";
            _view = new BridgemateControl();
            this.bcsService = bcs;
            this.dialogService = dialogService;

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
            bcsService.SendMovementData(MainViewModel.LoadedSession);
            Console.WriteLine("Database successfully updated!");
        }

        public void StartBCS() {

            var database = MainViewModel.LoadedSession?.DatabaseFilepath;

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = BCSFileName,
                Arguments = bcsArgs.Replace("{database}", database)
            };

            var process = new Process() { StartInfo = startInfo };

            try
            {
                process.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot start BCS, exception:" + e.Message);
            }
        }

        public void RetrieveResults() {
            try
            {
                bcsService.RetrieveResults(MainViewModel.LoadedSession, MainViewModel.LoadedTournament);
                Console.WriteLine("Results retrieved sucessfully!");
            }
            catch (InconsistentModelException e)
            {
                Console.WriteLine("Incosistent DB: " + e.InconsistencyType);
                var q = dialogService.GetYesOrNo($"There are more {e.InconsistencyType} in the database than in the tournament. \n" +
                    $"Do you want to add {e.InconsistencyType} to make up the diference?");

                if(q == DialogResult.Yes)
                {
                    Action a = null;
                    var iterations = e.CountInDB - e.CountInModel;

                    if(e.InconsistencyType == InconsistentModelException.ModelDBInconsistencyType.Pairs)
                    {
                        a = () => { MainViewModel.GetViewModel<PlayersViewModel>().AddPair(); };
                        
                    }
                    else if(e.InconsistencyType == InconsistentModelException.ModelDBInconsistencyType.Sections)
                    {
                        a = () => { MainViewModel.GetViewModel<SectionsViewModel>().AddSection(); };
                    }

                    for (int i = 0; i < iterations; i++)
                    {
                        a.Invoke();
                    }
                    RetrieveResults();
                }
            }

        }

    }
}
