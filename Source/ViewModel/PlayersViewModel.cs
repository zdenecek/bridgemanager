using BridgeManager.Source.Model;
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

        public Command AddPlayerCommand { get; private set; }
        public Command RemovePlayerCommand { get; private set; }
        public Command AddPairCommand { get; private set; }
        public Command RemovePairCommand { get; private set; }

        public PlayersViewModel(MainWindowViewModel mainController) : base(mainController) {
            PlayersControl view = new PlayersControl();
            this._view = view;

            this.Header = "Players";

            var tournament = MainViewModel.LoadedTournament;

            this.AddPlayerCommand = new DelegateCommand(() => AddPlayer(tournament));
            this.RemovePlayerCommand = new DelegateCommand<Player>(p => RemovePlayer(tournament, p));
            this.AddPairCommand = new DelegateCommand(() => AddPair(tournament));
            this.RemovePairCommand = new DelegateCommand<Pair>(p => RemovePair(tournament, p));
        }

        public Player AddPlayer(Tournament tournament) {
            Player player = new Player(tournament.Players.Count + 1) 
            {
                Name = Strings.Get("unknown_player") 
            };
            tournament.Players.Add(player);
            return player;
        }

        public void RemovePlayer(Tournament tournament, Player player) {
            if (player == null) {
                Console.WriteLine("Remove player: No player selected");
                return;
            }
            else if (player.Pair != null) {
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

        public Pair AddPair(Tournament tournament) {

            int number = tournament.Pairs.Count + 1;

            Pair pair = new Pair(number) {
                Player1 = AddPlayer(tournament),
                Player2 = AddPlayer(tournament)
            };

            tournament.Pairs.Add(pair);

            return pair;
        }
        public void RemovePair(Tournament tournament, Pair pair) {

            if (pair == null) {
                Console.WriteLine("Remove pair: No Pair Selected");
                return;
            }

            tournament.Pairs.Remove(pair);
            pair.ClearReference();
            RemovePlayer(tournament, pair.Player1);
            RemovePlayer(tournament, pair.Player2);

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
