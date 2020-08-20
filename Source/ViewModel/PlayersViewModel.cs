using BridgeManager.Source.Cultures;
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
using System.Windows.Data;
using System.Windows.Input;

namespace BridgeManager.Source.ViewModel {


    public class PlayersViewModel : ViewModelBase {

        public ObservableCollection<Player> Players { get => MainViewModel.LoadedTournament?.Players; }

        public ObservableCollection<Pair> Pairs { get => MainViewModel.LoadedTournament?.Pairs; }

        public Command AddPlayerCommand { get; private set; }
        public Command RemovePlayerCommand { get; private set; }
        public Command AddPairCommand { get; private set; }
        public Command RemovePairCommand { get; private set; }
        public Command AddMissingPairCommand { get; private set; }

        public override string Header { get => Properties.Strings.players_title; }

        public PlayersViewModel(MainWindowViewModel mainController) : base(mainController) {
            PlayersControl view = new PlayersControl();
            this._view = view;

            this.AddPlayerCommand = new DelegateCommand(() => AddPlayer());
            this.RemovePlayerCommand = new DelegateCommand<Player>(RemovePlayer);
            this.AddPairCommand = new DelegateCommand(() => AddPair());
            this.RemovePairCommand = new DelegateCommand<Pair>(RemovePair);
            this.AddMissingPairCommand = new DelegateCommand(() => AddPausePair());

            MainViewModel.PropertyChanged += (s, a) => { OnPropertyChanged("Players"); OnPropertyChanged("Pairs"); };
        }

        public Player AddPlayer() {

            if(IsLoaded == false)
            {
                Console.WriteLine(Properties.Strings._tournament_not_loaded);
                return null;
            }

            var tournament = MainViewModel.LoadedTournament;

            Player player = new Player(tournament.Players.Count + 1)
            {
                Name = Properties.Strings.players_default_name
            };
            tournament.Players.Add(player);
            return player;
        }

   

        public void RemovePlayer(Player player) {
            
            if (IsLoaded == false)
            {
                Console.WriteLine(Properties.Strings._tournament_not_loaded);
                return;
            }
            if (player == null) {
                Console.WriteLine(Properties.Strings.player_remove_player_no_player_selected);
                return;
            }
            var tournament = MainViewModel.LoadedTournament;

            if (tournament.Pairs.Where(pair => pair.Player1.Equals(player) || pair.Player2.Equals(player))
                .Count() != 0)
            {
                Console.WriteLine(Properties.Strings.player_remove_player_player_in_pair);
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

        public Pair AddPausePair()
        {
            var pair = AddPair();
            pair.IsMissing = true;
            //TODO Localize
            pair.Player1.Name = "Pause";
            pair.Player2.Name = "Pause";
            pair.Name = "Pause";
            foreach (var section in this.MainViewModel.LoadedSession.Sections )
            {
                section.MissingPair = pair;
            }
            return pair;
        }

        public Pair AddPair() {

            if (IsLoaded == false)
            {
                Console.WriteLine(Properties.Strings._tournament_not_loaded);
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
                Console.WriteLine(Properties.Strings._tournament_not_loaded);
                return;
            }
            var tournament = MainViewModel.LoadedTournament;
            if (pair == null) {
                Console.WriteLine(Properties.Strings.player_remove_pair_no_pair_selected);
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
    }
}
