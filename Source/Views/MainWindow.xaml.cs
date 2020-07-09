using BridgeManager.Source.ViewModel;
using BridgeManager.Source.Model;
using System.Windows;

namespace BridgeManager {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {


        private MainWindowViewModel mainController;

        public MainWindowViewModel MainController { get => mainController;}
    
        public MainWindow(MainWindowViewModel mainViewModel) {
            InitializeComponent();
            this.mainController = mainViewModel;
        }

        #region MenuEventHandlers

        private async void AddMovementMenu_Click(object sender, RoutedEventArgs e) {
           // await MainController.MovementController.AddMovement();
        }

        private void SetBMFile_Click(object sender, RoutedEventArgs e) {
          //  MainController.BridgemateController.SetBMDatabaseFile();
        }

    



        #endregion

    }
}
