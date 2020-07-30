using BridgeManager.Source.Model;
using BridgeManager.Source.Services.Dialog;
using BridgeManager.Source.Services.Files;
using BridgeManager.Source.Services.Movements;
using BridgeManager.Source.ViewModel.Commands;
using BridgeManager.Source.Views;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BridgeManager.Source.ViewModel
{
    public class MovementsViewModel : ViewModelBase {

        private IMovementsService movementsService;
        private IFileService fileService;
        private IDialogService dialogService;

        private ObservableCollection<Movement> _movements;
        public ObservableCollection<Movement> Movements { get => _movements; }

        public Command AddMovementCommand { get; set; }

        public override string Header { get => Properties.Strings.movements_title; }

        public MovementsViewModel(
            MainWindowViewModel mainController, 
            IMovementsService movementsService,
            IFileService fileService,
            IDialogService dialogService) : base(mainController) 
        {
            _movements = new ObservableCollection<Movement>();
            _view = new MovementsControl();
          
            this.movementsService = movementsService;
            this.fileService = fileService;
            this.dialogService = dialogService;
           
            this.AddMovementCommand = new DelegateCommand(AddMovement);
        }

        public async void AddMovement() {

            string filepath = dialogService.GetExistingFilepath();

            if (filepath == String.Empty) return;
            else await AddMovement(filepath);

        }

        public async Task AddMovement(string filepath) {

            //@TODO add checks
            //@TODO implement async
            
            string loadedText;

            try {
                loadedText = await fileService.ParseTextFile(filepath);
            }
            catch (Exception e) {
                Console.WriteLine(Properties.Strings.movements_add_movement_error + e.Message);
                return;
            }

            var m = await movementsService.ParseMovementFromFile(loadedText, filepath);

            if(m != null) _movements.Add(m);
        }

    }
}
