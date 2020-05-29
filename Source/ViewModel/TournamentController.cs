using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BridgeManager.Source.Model;

namespace BridgeManager.Source.ViewModel
{
    

    public class TournamentController : DelegateController { 

        private Tournament loadedTournament;
        private Session loadedSession;
        
        public Tournament LoadedTournament { get => loadedTournament; set => loadedTournament = value; }
        public Session LoadedSession { get => loadedSession; set => loadedSession = value; }

        public TournamentController(MainController mainController) : base(mainController) {
            this.LoadedTournament = new Tournament();
            this.LoadedSession = LoadedTournament.Sessions[0];
        }

        public Section AddSection() {
            return LoadedSession.addSection();
        }
    }
}
