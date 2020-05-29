using BridgeManager.Source.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BridgeManager.Source.ViewModel {


    public class PlayerController : DelegateController {

        public PlayerController(MainController mainController) : base(mainController) {

        }

        public Player AddPlayer() {

            Tournament tournament = MainController.LoadedTournament;
            Player player = new Player(tournament.Players.Count + 1){Name = Local.Get("unknown_player")};
            tournament.Players.Add(player);

            return player;

        }

        public void RemovePlayer(Player player) {

            if(player == null) {
                Console.WriteLine("Remove player: No player selected");
                return;
            }

            Tournament tournament = MainController.LoadedTournament;
            tournament.Players.Remove(player);

            if(player.Number > 0)
            foreach (Player p in from p in tournament.Players
                                  where p.Number > player.Number
                                  select p) {
                p.Number -= 1;
            }
    
        }

        public void ChangePlayerNumber(Player player, int newNumber) {

            Tournament tournament = MainController.LoadedTournament;

            if (player.Number == newNumber) return;
            else {
                Console.WriteLine("Changing player number");
                if (tournament.Players.Count(pl => pl.Number == newNumber) > 0) {
                    Console.WriteLine("There is a player with this id already");
                    MessageBoxResult res = MessageBox.Show("A player of the same number already exists, do you want to swap numbers?", "Change player number", MessageBoxButton.YesNoCancel);
                    if (res.Equals(MessageBoxResult.Cancel)) {
                        Console.WriteLine("Changing number cancelled."); return;
                    }
                    else {

                        if (res.Equals(MessageBoxResult.Yes)) {
                            foreach (Player p in from p in tournament.Players
                                                 where p.Number == newNumber
                                                 select p) {
                                p.Number = player.Number;
                            }
                        }
                        player.Number = newNumber;
                        MainController.MainWindow.dgPlayers.Items.Refresh(); //??
                    }
                }
            }
        }


        public Pair AddPair() {

            Tournament tournament = MainController.LoadedTournament;

            Pair pair = new Pair(tournament.Pairs.Count + 1) {
                Player1 = AddPlayer(),
                Player2 = AddPlayer()
            };

            tournament.Pairs.Add(pair);

            return pair;
        }

    }
}
