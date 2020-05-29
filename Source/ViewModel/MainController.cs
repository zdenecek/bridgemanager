using BridgeManager.Source.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BridgeManager.Source.ViewModel {
        public class MainController : INotifyPropertyChanged {

        public ObservableCollection<Player> Players { get => LoadedTournament.Players; }


        public TournamentController TournamentController { get; }
        public MovementController MovementController { get; }
        public BridgemateController BridgemateController { get; }
        public PlayerController PlayerController { get; }
        public Tournament LoadedTournament { get => TournamentController.LoadedTournament;  }
        public MainWindow MainWindow { get; }

        public MainController(MainWindow view) {

            MainWindow = view;
            
            this.MovementController = new MovementController(this);
            this.TournamentController = new TournamentController(this);
            this.BridgemateController = new BridgemateController(this);
            this.PlayerController = new PlayerController(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string name = null) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
       
    }
}
