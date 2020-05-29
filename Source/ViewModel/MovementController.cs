using BridgeManager.Source.IO;
using BridgeManager.Source.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.ViewModel {
    public class MovementController : DelegateController {

        private List<Movement> movements;

        public List<Movement> Movements { get => movements; }

        public MovementController(MainController mainController) : base(mainController) {
            this.movements = new List<Movement>();
        }



        //IMPLEMENT ASYNC LATER
        public async Task AddMovement() {

            OpenFileDialog fileDialog = new OpenFileDialog();
            if(fileDialog.ShowDialog() == true) {
                movements.Add(await MovementFileParser.ParseMovementFromFile(fileDialog.FileName));
            }
        }

    }
}
