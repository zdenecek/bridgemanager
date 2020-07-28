using BridgeManager.Source.Model.Scoring;
using BridgeManager.Source.Services.Scoring;
using BridgeManager.Source.ViewModel.Commands;
using BridgeManager.Source.Views;
using System;
using System.Collections.ObjectModel;

namespace BridgeManager.Source.ViewModel
{
    public class ScoringViewModel : ViewModelBase
    {

        private IScoringService scoringService;
      
        public Command CreateScoresCommand { get; private set; }

        public ObservableCollection<Score> PartialScores { get => MainViewModel.LoadedSession?.IntermediateScores; }

        public ObservableCollection<Score> Scores { get => MainViewModel.LoadedSession?.FinalScores; }

        public ScoringViewModel(MainWindowViewModel mainController, IScoringService scoringService) : base(mainController) {
            Header = "Scores";
            _view = new ScoringControl();
            this.scoringService = scoringService;
            this.CreateScoresCommand = new DelegateCommand(CreateScores);
        }

        public void CreateScores()
        {
            try
            {
                scoringService.CreateIntermediateScores(MainViewModel.LoadedSession);
                scoringService.CreateSessionScores(MainViewModel.LoadedSession);
            }
            catch(Exception e)
            {
                Console.WriteLine("Cannot create scores, error: " + e.Message);
            }
            
        }

       

               

    }
}
