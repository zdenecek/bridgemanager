using BridgeManager.Source.IO;
using BridgeManager.Source.IO.MovementParsing;
using BridgeManager.Source.Model;
using BridgeManager.Source.ViewModel.Commands;
using BridgeManager.Source.Views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.ViewModel {
    public class MovementsViewModel : ViewModelBase {

        private ObservableCollection<Movement> movements;

        public ObservableCollection<Movement> Movements { get => movements; }
        public MovementFileParser movementFileParser; 

        public Command AddMovementCommand { get; set; }

        public MovementsViewModel(MainWindowViewModel mainController) : base(mainController) {
            this.movements = new ObservableCollection<Movement>();
            this._view = new MovementsControl();
            Header = "Movements";

            this.movementFileParser = new MovementFileParser();

            this.AddMovementCommand = new DelegateCommand(AddMovement);
        }

        public async void AddMovement() {

            string filepath = DialogService.FileDialog();

            if (filepath == String.Empty) return;
            else await AddMovement(filepath);

        }

        //IMPLEMENT ASYNC LATER
        public async Task AddMovement(string filepath) {

            
            string loadedText;

            try {
                loadedText = await TextFileHandler.ParseTextFile(filepath);
            }
            catch (Exception e) {
                Console.WriteLine($"Error reading the movement file{filepath}, exception:{e.Message}");
                return;
            }

            var m = await movementFileParser.ParseMovementFromFile(loadedText, filepath);

            if(m != null) movements.Add(m);
        }

    }
}
