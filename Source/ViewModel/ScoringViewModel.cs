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

        public override string Header { get => Properties.Strings.scoring_title; }

        public ScoringViewModel(MainWindowViewModel mainController, IScoringService scoringService) : base(mainController) {
           
            _view = new ScoringControl();
            this.scoringService = scoringService;
            this.CreateScoresCommand = new DelegateCommand(CreateScores);
        }

        public void CreateScores()
        {
            try
            {
                scoringService.CreateIntermediateSessionScores(MainViewModel.LoadedSession);
                scoringService.CreateSessionScores(MainViewModel.LoadedSession);
            }
            catch(Exception e)
            {
                Console.WriteLine(Properties.Strings.scoring_create_scores_error + e.Message);
            }
        }
    }
}
