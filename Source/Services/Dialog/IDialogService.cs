using BridgeManager.Source.Model;
using System.Collections.Generic;

namespace BridgeManager.Source.Services.Dialog
{
    public interface IDialogService
    {
        DialogResult GetYesOrNo(string query);
        string GetExistingFilepath();
        string GetNewFilepath();
        Movement SelectMovement(IEnumerable<Movement> movements);
        void OpenSettings();
    }
}