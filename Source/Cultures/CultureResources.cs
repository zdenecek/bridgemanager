using BridgeManager.Source.Component;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using BridgeManager.Properties;

namespace BridgeManager.Source.Cultures
{
    public class CultureResources : ObservableObject
    {
        public static event EventHandler CultureChanged;

        public Strings GetResourceInstance()
        {
            return new Properties.Strings();
        }

        private static ObjectDataProvider _dataProvider;
        private static ObjectDataProvider DataProvider
        {
            get
            {
                if (_dataProvider == null)
                {
                    _dataProvider = App.Current.FindResource("Strings") as ObjectDataProvider;
                }
                return _dataProvider;
            }
        }

        public static void ChangeCulture(string CultureName)
        {
            var culture = new CultureInfo(CultureName);
            Properties.Strings.Culture = culture;
            Debug.WriteLine("Changing culture to: " + culture.Name);
            DataProvider.Refresh();
            OnCultureChanged();
        }

        private static void OnCultureChanged()
        {
            CultureChanged?.Invoke(null, new EventArgs());
        }
    }
}

