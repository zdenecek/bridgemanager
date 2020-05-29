using BridgeManager.Source.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace BridgeManager.Source.ViewModel
{
    public class BridgemateController : DelegateController
    {
        private string BMDatabaseFile;

        public BridgemateController(MainController mainController) : base(mainController) {

        }


        public void SetBMDatabaseFile() {

            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true) {
                this.BMDatabaseFile = fileDialog.FileName;
            }
        }

        public bool SendDataToBM(Session session) {

            OleDbCommandBuilder builder = new OleDbCommandBuilder();
            return true;
        }


    }
}
