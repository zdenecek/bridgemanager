using BridgeManager.Source.Cultures;
using BridgeManager.Source.ViewModel.Dialog.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.Utilities
{
    public class SettingsLoader
    {

        public static List<SettingsCategory> LoadSettings()
        {
            List<SettingsCategory> categories = new List<SettingsCategory>();
            setup(ref categories);
            return categories;

            void setup(ref List<SettingsCategory> list)
            {
                var interf = new SettingsCategory("Interface");
                interf.Options.Add(
                    new DropdownSettingsOption(
                        "ui_language",
                        "Language",
                        new Dictionary<string, string> {
                        { "Czech", "cs-CZ" },
                        { "English", "en"  }},
                        (s) => { CultureResources.ChangeCulture(s); }
                        )
                    );

                list.Add(interf);

            }
        }
    }
}
