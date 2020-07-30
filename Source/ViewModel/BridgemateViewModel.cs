using System;
using BridgeManager.Source.ViewModel.Commands;
using BridgeManager.Source.Views;
using BridgeManager.Source.Services.Database;
using System.Diagnostics;
using BridgeManager.Source.Services.Dialog;

namespace BridgeManager.Source.ViewModel
{
    public class BridgemateViewModel : ViewModelBase
    {   
        private IBCSService bcsService;
        private IDialogService dialogService;

        public Command SendCurrentSessionCommand { get; private set; }
        public Command StartBCSCommand { get; private set; }
        public Command RetrieveResultsCommand { get; private set; }

        public override string Header { get => Properties.Strings.bridgemate_title; }

        public BridgemateViewModel(MainWindowViewModel mainController, IBCSService bcs, IDialogService dialogService) : base(mainController) {
           
            _view = new BridgemateControl();
            this.bcsService = bcs;
            this.dialogService = dialogService;

            this.SendCurrentSessionCommand = new DelegateCommand(SendDataToBM);
            this.StartBCSCommand = new DelegateCommand(StartBCS);
            this.RetrieveResultsCommand = new DelegateCommand(RetrieveResults);
        }

        public void SendDataToBM() {
            //@TODO Add checks
            if(MainViewModel.LoadedSession.DatabaseFilepath == null) {
                Console.WriteLine(Properties.Strings.bridgemate_send_data_no_data);
                return;
            }
            bcsService.SendMovementData(MainViewModel.LoadedSession);
            Console.WriteLine(Properties.Strings.bridgemate_send_data_success);
        }

        public void StartBCS() {

            var path = Properties.Settings.Default.path_bcs;
            var args = Properties.Settings.Default.misc_bcs_args;

            var database = MainViewModel.LoadedSession?.DatabaseFilepath;

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = path,
                Arguments = args.Replace("{database}", database)
            };

            var process = new Process() { StartInfo = startInfo };

            try
            {
                process.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(Properties.Strings.bridgemate_start_BCS_error + e.Message);
            }
        }

        public void RetrieveResults() {
            //@TODO Add checks
            try
            {
                bcsService.RetrieveResults(MainViewModel.LoadedSession, MainViewModel.LoadedTournament);
                Console.WriteLine(Properties.Strings.bridgemate_retrieve_results_success);
            }
            catch (InconsistentModelException e)
            {
                Console.WriteLine(Properties.Strings.bridgemate_retrieve_results_inconsistent_db + e.InconsistencyType);

                var message = e.InconsistencyType == InconsistentModelException.ModelDBInconsistencyType.Pairs ?
                    Properties.Strings.bridgemate_retrieve_results_query_incosistent_pairs : Properties.Strings.bridgemate_retrieve_results_query_incosistent_sections;

                var q = dialogService.GetYesOrNo(message);

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
