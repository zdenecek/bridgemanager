using BridgeManager.Source.Model;
using BridgeManager.Source.ViewModel;
//using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BridgeManager.Source.IO {
    public static class DialogService {

        public static Movement MovementSelector(IEnumerable<Movement> movements) {

            MovementSelectorWindowViewModel selector = new MovementSelectorWindowViewModel(movements);

            selector.MovementSelectorWindow.ShowDialog();

            return selector.MovementSelectorWindow.lvMovements.SelectedItem as Movement;

        }

        public static string FileDialog() {
          
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();
            return fileDialog.FileName;
       
        }

        public static string SaveFileDialog()
        {

            var fileDialog = new SaveFileDialog();
            fileDialog.ShowDialog();
            return fileDialog.FileName;
        }


    }
}
