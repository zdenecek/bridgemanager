using BridgeManager.Source.ViewModel;
using BridgeManager.Source.ViewModel.Dialog.Settings;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BridgeManager.Source.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        public List<SettingsCategory> LoadSettings()
        {
            List<SettingsCategory> categories = new List<SettingsCategory>();
            setup(ref categories);
            return categories;
        }

        private void setup(ref List<SettingsCategory> list)
        {
            var interf = new SettingsCategory("Interface");
            interf.Options.Add(
                new DropdownSettingsOption(
                    "ui_language", 
                    "Language", 
                    new Dictionary<string, string> {
                        { "Czech", "cs-CZ" },
                        { "English", "en"  }},
                    (s) => { Thread.CurrentThread.CurrentUICulture = new CultureInfo(s);
                        Thread.CurrentThread.CurrentCulture = new CultureInfo(s);
                    }));

            list.Add(interf);

        }
    }
}
