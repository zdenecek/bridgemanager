using BridgeManager.Source.Model;
using BridgeManager.Source.Services.Settings;
using BridgeManager.Source.ViewModel.Commands;
using BridgeManager.Source.ViewModel.Dialog.Settings;
using BridgeManager.Source.Views;
using BridgeManager.Source.Views.Dialog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BridgeManager.Source.ViewModel.Dialog.Settings {
    public class SettingsWindowViewModel
    {
        public ObservableCollection<SettingsCategory> Categories { get; }

        public SettingsWindow SettingsWindow { get; private set; }

        public Command OKCommand { get; private set; }
        public Command ApplyCommand { get; private set; }
        public Command DefaultCommand { get; private set; }



        public SettingsWindowViewModel(ISettingsService settingsLoader) {

            var settings = settingsLoader.LoadSettings();
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

            Console.WriteLine(Thread.CurrentThread.CurrentUICulture + ""+ Thread.CurrentThread.CurrentCulture);
            SettingsWindow.Hide();
        }
        
    }

    

    
}
