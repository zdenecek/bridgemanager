using BridgeManager.Source.ViewModel;
using BridgeManager.Source.IO;
using BridgeManager.Source.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BridgeManager {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {


        private MainController mainController;

        public MainController MainController { get => mainController;}
    
        public MainWindow() {
            InitializeComponent();

       

            this.mainController = new MainController(this);


            this.DataContext = MainController;



        }

        #region UserInput

        private bool boolDialog(string text) {
            return MessageBox.Show(text, "Bridge Manager", MessageBoxButton.YesNo).Equals(MessageBoxResult.Yes);
        }

        #endregion

        #region MenuEventHandlers

        private async void AddMovementMenu_Click(object sender, RoutedEventArgs e) {
            await MainController.MovementController.AddMovement();
        }

        private void SetBMFile_Click(object sender, RoutedEventArgs e) {
            MainController.BridgemateController.SetBMDatabaseFile();
        }

        private void AddSectionMenu_Click(object sender, RoutedEventArgs e) {
            MainController.TournamentController.AddSection();
        }

        private void AddPair_Click(object sender, RoutedEventArgs e) {
            MainController.PlayerController.AddPair();
        }

        private void AddPlayer_Click(object sender, RoutedEventArgs e) {
            MainController.PlayerController.AddPlayer();
        }

        private void RemovePlayer_Click(object sender, RoutedEventArgs e) { 
            MainController.PlayerController.RemovePlayer(dgPlayers.SelectedItem as Player);
           
        }

        #endregion

        #region dataGrids
        
        private void dgPlayers_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e) {
            if (e.Column.Header.ToString() == "Id") {
                int old = ((Player)dgPlayers.SelectedItem).Number;
                int _new = 0;
                
                if (Int32.TryParse((dgPlayers.SelectedCells[0].Column.GetCellContent(e.Row) as TextBox).Text, out _new)) {
                    Console.WriteLine($"Change player numbers: old={old} new={_new}");
                    MainController.PlayerController.ChangePlayerNumber(e.Row.Item as Player, _new);
                }
                else {
                    Console.WriteLine("Invalid input for player number");
                    (dgPlayers.SelectedCells[0].Column.GetCellContent(e.Row) as TextBox).Text = old.ToString();
                }
            }

            
        }

        #endregion


    }
}
