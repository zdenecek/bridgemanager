using BridgeManager.Source.Model;
using BridgeManager.Source.ViewModel;
//using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace BridgeManager.Source.Services {
    public class DialogService : IDialogService
    {

        public Movement SelectMovement(IEnumerable<Movement> movements)
        {
            MovementSelectorWindowViewModel selector = new MovementSelectorWindowViewModel(movements);

            selector.MovementSelectorWindow.ShowDialog();

            return selector.MovementSelectorWindow.lvMovements.SelectedItem as Movement;
        }

        public string GetExistingFilepath()
        {

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();
            return fileDialog.FileName;

        }

        public string GetNewFilepath()
        {

            var fileDialog = new SaveFileDialog();

            fileDialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            fileDialog.FilterIndex = 1;
            fileDialog.RestoreDirectory = true;
            fileDialog.FileName = "tournament";

            fileDialog.ShowDialog();
            return fileDialog.FileName;
        }

        public DialogResult GetYesOrNo(string query)
        {
            MessageBoxResult res = MessageBox.Show(query, "Query", MessageBoxButton.YesNoCancel);
            switch (res)
            {
                case MessageBoxResult.Yes: return DialogResult.Yes;
                case MessageBoxResult.No: return DialogResult.No;
                default: return DialogResult.Cancel;
            }
        }
    }

    public enum DialogResult
    {
        Yes,
        No,
        Cancel

           
    }
}
