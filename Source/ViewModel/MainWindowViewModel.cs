using BridgeManager.Source.Component;
using BridgeManager.Source.IO;
using BridgeManager.Source.IO.Database;
using BridgeManager.Source.Model;
using BridgeManager.Source.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace BridgeManager.Source.ViewModel {
    public partial class MainWindowViewModel : ObservableObject {

        private ObservableCollection<ViewModelBase> _viewModels;
        private Tournament loadedTournament;
        private Session loadedSession;

        private SerializationService xmlService;

        public Tournament LoadedTournament { get => loadedTournament; set => loadedTournament = value; }
        public Session LoadedSession { get => loadedSession; set { loadedSession = value; OnPropertyChanged(); } }

        public ObservableCollection<ViewModelBase> ViewModels { get => _viewModels; }
        public MainWindow MainWindow { get; private set; }


        public Command OpenCommand { get; set; }
        public Command CreateNewCommand { get; set; }
        public Command SaveAsCommand { get; set; }


        public void debugCode() {
            /*
            LoadedSession.DatabaseFilepath = @"C:\Users\zdnek\ME\Bridge\BridgeManager\testfiles\BLANK_BM - Copy.bws";
            await Get<MovementsViewModel>().AddMovement(@"C:\Users\zdnek\ME\Bridge\BridgeManager\testfiles\klub3.DAT");
            LoadedSession.Sections[0].Movement = Get<MovementsViewModel>().Movements[0];
            */
            var ser = new SerializationService();
            //ser.Serialize(loadedTournament, @"C:\Users\zdnek\ME\Bridge\BridgeManager\testfiles\test1.txt");
            LoadedTournament = ser.Deserialize(@"C:\Users\zdnek\ME\Bridge\BridgeManager\testfiles\test1.txt");
            LoadedSession = LoadedTournament.Sessions.First();

            DialogService.FileDialog();
        }

        public MainWindowViewModel() {

            LoadedTournament = new Tournament();

            _viewModels = new ObservableCollection<ViewModelBase>();
            _viewModels.Add(new SessionsViewModel(this));
            _viewModels.Add(new PlayersViewModel(this));
            _viewModels.Add(new MovementsViewModel(this));
            _viewModels.Add(new SectionsViewModel(this));
            _viewModels.Add(new BridgemateViewModel(this));

            MainWindow = new MainWindow(this)
            {
                DataContext = this
            };
            MainWindow.Show();

            Console.SetOut(new ConsoleOutputter(MainWindow.ConsoleTextBox));
            Console.WriteLine("Welcome to BridgeManager v. 0.1");

            
            Get<PlayersViewModel>().AddPlayer(LoadedTournament).Name = "Marek Novák";
            LoadedSession = Get<SessionsViewModel>().AddSession(LoadedTournament);
            Get<SectionsViewModel>().AddSection();

            LoadedTournament.Players.CollectionChanged += CollectionChanged<Player>;
            LoadedTournament.Pairs.CollectionChanged += CollectionChanged<Pair>;
            LoadedTournament.Sessions.CollectionChanged += CollectionChanged<Session>;
            //Sections done automatically
            //////////////////////////////////////////////////////////////////////////////////////////
            // MainWindow.dgSection.ItemsSource = TournamentController.LoadedSession.Sections;

            this.CreateNewCommand = new DelegateCommand(() => Load(new Tournament()));
            this.OpenCommand = new DelegateCommand(() => { });
            this.SaveAsCommand = new DelegateCommand(() => { });

            this.debugCode();
        }

        public T Get<T>() where T : ViewModelBase {
            return _viewModels.OfType<T>().First<T>();
        }

      

        public void _New()
        {

        }


        public void Open()
        {
            var file = DialogService.FileDialog();
        }

        public void Load(Tournament t)
        {
            if (t.Sessions.Count == 0) Get<SessionsViewModel>().AddSession(t);
            LoadedTournament = t;
            LoadedSession = t.Sessions.First(); //redo to the first not finished or the last if all are finished.

            foreach(Session ses in t.Sessions)
            {
                foreach(Section sec in ses.Sections)
                {
                    Get<MovementsViewModel>().Movements.Add(sec.Movement);
                }
            }
        }

        #region eventListeners

        /// <summary>
        /// For Collection of IndexedObjects to trigger OnNumberChange whenever one happens;
        /// //@TODO Implement renumbering
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectionChanged<T>(object sender, NotifyCollectionChangedEventArgs e) where T : IndexedObject {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add) {
                var en = e.NewItems.GetEnumerator();
                Console.WriteLine(typeof(T).Name + " added.");
                while (en.MoveNext()) {

                    if (typeof(T) == typeof(Session)) (en.Current as Session).Sections.CollectionChanged += CollectionChanged<Section>;
                    (en.Current as T).PropertyChanged += delegate (object t, PropertyChangedEventArgs ea) {
                        if (ea.PropertyName == "Number") OnNumberChanged(sender as IEnumerable<T>, t as T, (ea as AugmentedPropertyChangedEventArgs).OldValue as int? ?? -1);

                    };
                }
            }
        }

        #endregion

        private bool NumberChangeOperation = false;

        public void OnNumberChanged<T>(IEnumerable<T> sender, T unit, int oldId) where T : IndexedObject {

            if (this.NumberChangeOperation) return;
            else if (unit.Number == oldId) return;
            else this.NumberChangeOperation = true;

            string s = typeof(T).Name;

            Console.WriteLine(s + " number edit");

            if (sender.Count(pl => pl.Number == unit.Number) > 1) {
                Console.WriteLine($"There is a {s} with this id already");
                MessageBoxResult res = MessageBox.Show($"A {s} of the same number already exists, do you want to swap numbers?", $"Change {s} number", MessageBoxButton.YesNoCancel);
                if (res.Equals(MessageBoxResult.Cancel)) {
                    //Revert
                    Console.WriteLine("Changing number cancelled.");
                    unit.Number = oldId;
                    return;
                }
                else if (res.Equals(MessageBoxResult.Yes)) {
                    //Swap
                    foreach (T p in from p in sender
                                    where p.Number == unit.Number && !p.Equals(unit)
                                    select p) {
                        p.Number = oldId;
                    }
                }
                //else Keep
            }

            this.NumberChangeOperation = false;
        }

    }

    
}
