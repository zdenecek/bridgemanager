using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.ViewModel.Dialog.Settings
{
    public class SettingsCategory
    {
        public string Header { get; set; }

        public ObservableCollection<SettingsItem> Options { get; set; }

        public SettingsCategory(string Header = "???")
        {
            this.Header = Header;
            Options = new ObservableCollection<SettingsItem>();
        }

        public void ApplyAll()
        {
            foreach(var option in Options)
            {
                option.Apply();
            }
        }

    }
}
