using BridgeManager.Source.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.Model
{
    public class Tournament
    {
        public enum TournamentType {
            Individual,
            Pairs,
            Teams,
            
        };

        private string name;
        private TournamentType tournamentType;
        private ObservableCollection<Session> sessions;
        private ObservableCollection<Player> players;
        private ObservableCollection<Pair> pairs;
        private ObservableCollection<Team> teams;

        public string Name { get => name; set => name = value; }
        public TournamentType TournamentType1 { get => tournamentType; set => tournamentType = value; }
        public ObservableCollection<Session> Sessions { get => sessions; set => sessions = value; }
        public ObservableCollection<Player> Players { get => players;  }
        public ObservableCollection<Pair> Pairs { get => pairs; set { pairs = value; } }
        public ObservableCollection<Team> Teams { get => teams; set => teams = value; }

        public Tournament() {
            this.sessions = new ObservableCollection<Session>();
            this.players = new ObservableCollection<Player>();
            this.pairs = new ObservableCollection<Pair>();
            this.teams = new ObservableCollection<Team>();

            this.tournamentType = TournamentType.Individual;
            ////
            Sessions.Add(new Session(1));
            Players.Add(new Player(1) { Name = "Marek Novák"});

        }

    }
}
