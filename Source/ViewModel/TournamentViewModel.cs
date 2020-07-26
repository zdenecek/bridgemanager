using BridgeManager.Source.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using BridgeManager.Source.ViewModel.Commands;
using BridgeManager.Source.Views;
using BridgeManager.Source.IO.Database;
using System.Diagnostics;
using BridgeManager.Source.Services.Database;
using BridgeManager.Source.Services;
using BridgeManager.Source.IO;

namespace BridgeManager.Source.ViewModel
{
    public class TournamentViewModel : ViewModelBase
    {
        private IDialogService dialogService;
        private ISerializationService xmlService;

        private string cachedPath;

        public Command OpenCommand { get; private set; }
        public Command NewCommand { get; private set; }
        public Command SaveAsCommand { get; private set; }
        public Command SaveCommand { get; private set; }

        public TournamentViewModel(MainWindowViewModel mainController, IDialogService dialog, ISerializationService serialization) : base(mainController) {
            Header = "Tournament";
            _view = new TournamentControl();

            xmlService = serialization;
            dialogService = dialog;

            this.NewCommand = new DelegateCommand(New);
            this.OpenCommand = new DelegateCommand(Open);
            this.SaveAsCommand = new DelegateCommand(SaveAs);
            this.SaveCommand = new DelegateCommand(Save);
        }

        public void New()
        {
            Console.WriteLine("Loading new tournament");
            MainViewModel.Load(new Tournament());
        }

        public void Open()
        {
            var path = dialogService.GetExistingFilepath();
            Open(path);
        }

        public void Open(string path)
        {
            try
            {
                var tournament = xmlService.Deserialize(path);
                MainViewModel.Load(tournament);
                cachedPath = path;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error loading tournament: " + e.Message);
                return;
            }
        }

        public void Save(string path)
        {
            try
            {
                if (MainViewModel.LoadedTournament == null)
                    Console.WriteLine("No tournament is loaded, cannot save.");
                else
                {
                    xmlService.Serialize(MainViewModel.LoadedTournament, path);
                    cachedPath = path;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid path: " + e.Message);             
            }
        }

        public void Save()
        {
            if (cachedPath == string.Empty)
                SaveAs();
            else
                Save(cachedPath);
        }

        public void SaveAs()
        {
            var path = dialogService.GetNewFilepath();
            Save(path);
        }


    }
}
