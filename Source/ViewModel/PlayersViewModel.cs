using BridgeManager.Source.Model;
using BridgeManager.Source.Services;
using BridgeManager.Source.ViewModel.Commands;
using BridgeManager.Source.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BridgeManager.Source.ViewModel {


    public class PlayersViewModel : ViewModelBase {

        public ObservableCollection<Player> Players { get => MainViewModel.LoadedTournament?.Players; }

        public ObservableCollection<Pair> Pairs { get => MainViewModel.LoadedTournament?.Pairs; }

        public Command AddPlayerCommand { get; private set; }
        public Command RemovePlayerCommand { get; private set; }
        public Command AddPairCommand { get; private set; }
        public Command RemovePairCommand { get; private set; }

        public PlayersViewModel(MainWindowViewModel mainController) : base(mainController) {
            PlayersControl view = new PlayersControl();
            this._view = view;
            this.Header = Properties.Strings.players_title;

            this.AddPlayerCommand = new DelegateCommand(() => AddPlayer());
            this.RemovePlayerCommand = new DelegateCommand<Player>(RemovePlayer);
            this.AddPairCommand = new DelegateCommand(() => AddPair());
            this.RemovePairCommand = new DelegateCommand<Pair>(RemovePair);

            MainViewModel.PropertyChanged += (s, a) => { OnPropertyChanged("Players"); OnPropertyChanged("Pairs"); };
        }

        public Player AddPlayer() {

            if(IsLoaded == false)
            {
                Console.WriteLine("No loaded tournament;");
                return null;
            }

            var tournament = MainViewModel.LoadedTournament;

            Player player = new Player(tournament.Players.Count + 1)
            {
                Name = "unknown_player"
            };
            tournament.Players.Add(player);
            return player;
        }

        public void RemovePlayer(Player player) {
            Console.WriteLine("RemoveP");

            if (IsLoaded == false)
            {
                Console.WriteLine("No loaded tournament;");
                return;
            }
            if (player == null) {
                Console.WriteLine("Remove player: No player selected");
                return;
            }
            var tournament = MainViewModel.LoadedTournament;

            if (tournament.Pairs.Where(pair => pair.Player1.Equals(player) || pair.Player2.Equals(player))
                .Count() != 0)
            {
                Console.WriteLine("Remove player: Cannot remove player in pair");
                return;
            }
                         
            tournament.Players.Remove(player);
            if (player.Number > 0)
                foreach (Player p in from p in tournament.Players
                                     where p.Number > player.Number
                                     select p) {
                    p.Number -= 1;
                }
        }

        public Pair AddPair() {

            if (IsLoaded == false)
            {
                Console.WriteLine("No loaded tournament;");
                return null;
            }
            var tournament = MainViewModel.LoadedTournament;

            int number = tournament.Pairs.Count + 1;

            Pair pair = new Pair(number) {
                Player1 = AddPlayer(),
                Player2 = AddPlayer()
            };

            tournament.Pairs.Add(pair);

            return pair;
        }
        public void RemovePair(Pair pair) {

            if (IsLoaded == false)
            {
                Console.WriteLine("No loaded tournament;");
                return;
            }
            var tournament = MainViewModel.LoadedTournament;
            if (pair == null) {
                Console.WriteLine("Remove pair: No Pair Selected");
                return;
            }

            tournament.Pairs.Remove(pair);
            RemovePlayer(pair.Player1);
            RemovePlayer(pair.Player2);

            foreach (Pair p in from p in tournament.Pairs
                               where p.Number >= pair.Number
                               select p) {
                p.Number -= 1;
            }
        }
        
        //Not implemented yet
        public List<Pair> CreatePairs(IList pls) {
            throw new NotImplementedException("Creating pairs not implemented yet");
            /*List<Pair> pairs = new List<Pair>();
            if (pls != null) {


                using (var players = (from p in pls.Cast<Player>()
                                      where p.Pair == null
                                      orderby p.Number
                                      select p).GetEnumerator()) {

                    Tournament tournament = MainViewModel.LoadedTournament;

                    int number = tournament.Pairs.Count + 1;

                    Player curr;

                    do {
                        curr = players.Current;
                        if (players.MoveNext()) {
                            Pair pair = new Pair(number++) {
                                Player1 = curr,
                                Player2 = players.Current
                            };
                            pairs.Add(pair);
                            tournament.Pairs.Add(pair);
                        }
                    } while (players.MoveNext());
                }
            }
            return pairs;
            */
        }

    }
}
