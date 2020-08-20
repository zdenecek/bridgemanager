using BridgeManager.Source.Utilities;
using BridgeManager.Source.ViewModel.Commands;
using BridgeManager.Source.Views.Dialog;
using System.Collections.ObjectModel;

namespace BridgeManager.Source.ViewModel.Dialog.Settings
{
    public class SettingsWindowViewModel
    {
        public ObservableCollection<SettingsCategory> Categories { get; }

        public SettingsWindow SettingsWindow { get; private set; }

        public Command OKCommand { get; private set; }
        public Command ApplyCommand { get; private set; }
        public Command DefaultCommand { get; private set; }



        public SettingsWindowViewModel() {

            var settings = SettingsLoader.LoadSettings();
            Categories = new ObservableCollection<SettingsCategory>(settings);
            

            OKCommand = new DelegateCommand(() => { ApplySettings(); Hide(); });
            ApplyCommand = new DelegateCommand(ApplySettings);

            
            SettingsWindow = new SettingsWindow();
            SettingsWindow.Closing += (s, e) => { e.Cancel = true; Hide(); };
            SettingsWindow.DataContext = this;
            
        }

        public void ApplySettings() {
            foreach(var category in Categories)
            {
                category.ApplyAll();
            }
            Properties.Settings.Default.Save();
        }

        public void Hide()
        {
            SettingsWindow.Hide();
        }
        
    }

    

    
}
