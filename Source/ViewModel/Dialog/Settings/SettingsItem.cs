using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BridgeManager.Source.ViewModel.Dialog.Settings
{
    public abstract class SettingsItem
    {
        protected abstract string ValueInUI { get; }

        private PropertyInfo setting;

        private Action<string> applyAction;

        protected SettingsItem(string settingName, Action<string> applyAction = null)
        {
            setting = Properties.Settings.Default.GetType().GetProperty(settingName);
            this.applyAction = applyAction;
        }

        public string Header { get; set; }
        public string Description { get; set; }
        public Control Control { get; protected set; }

        public void Apply()
        {
            setting.SetValue(Properties.Settings.Default, ValueInUI);
            applyAction?.Invoke(ValueInUI);
        }

        public string GetSettingsValue()
        {
            return setting.GetValue(Properties.Settings.Default) as string;
        }
    }

    public class DropdownSettingsOption : SettingsItem
    {
        protected Dictionary<string, string> Items;

        public DropdownSettingsOption( string settingName, string header, Dictionary<string,string> items, Action<string> onApply = null) : base(settingName, onApply)
        {
            Header = header;
            Items = items;
            Control = new ComboBox() {
            ItemsSource = Items.Keys};

            var currentValue = GetSettingsValue();
            (Control as ComboBox).SelectedItem = items.FirstOrDefault(x => x.Value == currentValue).Key;
        }

        protected override string ValueInUI { get => Items[(Control as ComboBox).Text]; }
    }
}
