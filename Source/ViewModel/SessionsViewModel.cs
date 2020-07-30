using BridgeManager.Source.Model;
using BridgeManager.Source.Services.Dialog;
using BridgeManager.Source.ViewModel.Commands;
using BridgeManager.Source.Views;
using System;
using System.Collections.ObjectModel;

namespace BridgeManager.Source.ViewModel
{
    public class SessionsViewModel : ViewModelBase {

        private IDialogService dialogService;

        public ObservableCollection<Session> Sessions { get => MainViewModel.LoadedTournament?.Sessions; }

        public Command AddSessionCommand { get; set; }
        public Command RemoveSessionCommand { get; set; }
        public Command AssignDatabaseFilepathCommand { get; set; }

        public override string Header { get => Properties.Strings.sessions_title; }

        public SessionsViewModel(MainWindowViewModel mainWindowViewModel,
            IDialogService dialogService) : base(mainWindowViewModel)
        {
            _view = new SessionsControl();
            

            this.dialogService = dialogService;

            this.AddSessionCommand = new DelegateCommand(() => AddSession());
            this.RemoveSessionCommand = new DelegateCommand<Session>(s=> RemoveSession(s));
            this.AssignDatabaseFilepathCommand = new DelegateCommand<Session>(AssignDatabaseFilepath);
        }

        public Session AddSession() {
            var tournament = MainViewModel.LoadedTournament;
            Session session = new Session(tournament.Sessions.Count + 1);
            tournament.Sessions.Add(session);
            return session;
        }

        public void RemoveSession(Session session) {
            var tournament = MainViewModel.LoadedTournament;
            if (session.Equals(MainViewModel.LoadedSession)) {
                Console.WriteLine("Cannot remove loaded session");
                return;
            }
            string success = "successful";
            string fail = "failed";
            string res = tournament.Sessions.Remove(session) ? success : fail;
            Console.WriteLine($"Remove session: {res }");
        }

        public void AssignDatabaseFilepath(Session session) {
            session.DatabaseFilepath = dialogService.GetExistingFilepath();
        }
    }
}
