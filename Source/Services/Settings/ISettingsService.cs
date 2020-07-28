using BridgeManager.Source.ViewModel.Dialog.Settings;
using System.Collections.Generic;

namespace BridgeManager.Source.Services.Settings
{
    public interface ISettingsService
    {
        List<SettingsCategory> LoadSettings();
    }
}