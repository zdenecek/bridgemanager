using BridgeManager.Source.IO;
using BridgeManager.Source.Model;
using BridgeManager.Source.ViewModel.Commands;
using BridgeManager.Source.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BridgeManager.Source.ViewModel {
    public class SessionsViewModel : ViewModelBase {

        public ObservableCollection<Session> Sessions { get => MainViewModel.LoadedTournament.Sessions; }


        public Command AddSessionCommand { get; set; }
        public Command RemoveSessionCommand { get; set; }
        public Command AssignDatabaseFilepathCommand { get; set; }

        public SessionsViewModel(MainWindowViewModel mainWindowViewModel) : base(mainWindowViewModel) {
            this._view = new SessionsControl();
            Header = "Sessions";

            this.AddSessionCommand = new DelegateCommand(() => AddSession(MainViewModel.LoadedTournament));
            this.RemoveSessionCommand = new DelegateCommand<Session>(sess => RemoveSession(MainViewModel.LoadedTournament, sess));
            this.AssignDatabaseFilepathCommand = new DelegateCommand<Session>(AssignDatabaseFilepath);

        }

        public Session AddSession(Tournament tournament) {
            Session session = new Session(tournament.Sessions.Count + 1);
            tournament.Sessions.Add(session);
            return session;
        }

        public void RemoveSession(Tournament tournament, Session session) {
            if(session.Equals(MainViewModel.LoadedSession)) {
                Console.WriteLine("Cannot remove loaded session");
                return;
            }
            string success = "successful";
            string fail = "failed";
            string res = tournament.Sessions.Remove(session) ? success : fail;
            Console.WriteLine($"Remove session: {res }");
        }

        public void AssignDatabaseFilepath(Session session) {
            session.DatabaseFilepath = DialogService.FileDialog();
        }
    }
}
