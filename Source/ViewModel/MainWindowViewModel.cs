using BridgeManager.Source.Component;
using BridgeManager.Source.Model;
using BridgeManager.Source.Services;
using BridgeManager.Source.Services.Dialog;
using BridgeManager.Source.ViewModel.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace BridgeManager.Source.ViewModel
{
    public class MainWindowViewModel : ObservableObject
    {

        private IDialogService dialogService;

        private bool NumberChangeOperation = false;
        private ObservableCollection<ViewModelBase> _viewModels;
        private Tournament _loadedTournament;
        private Session _loadedSession;

        public Tournament LoadedTournament { get => _loadedTournament; set { _loadedTournament = value; OnPropertyChanged(); } }
        public Session LoadedSession { get => _loadedSession; set { _loadedSession = value; OnPropertyChanged(); } }

        public bool IsLoaded { get => LoadedSession != null; }

        public ObservableCollection<ViewModelBase> ViewModels { get => _viewModels; }
        public MainWindow MainWindow { get; private set; }

        public Command OpenSetting { get; set; }

        public MainWindowViewModel(IDialogService dialogService, ISerializationService serializationService)
        {

            _viewModels = new ObservableCollection<ViewModelBase>();

            this.dialogService = dialogService;

            //Initialize commands before showing window because commands dont implement OnPropertyChanged()
            OpenSetting = new DelegateCommand(dialogService.OpenSettings);

            MainWindow = new MainWindow()
            {
                DataContext = this
            };

            MainWindow.Show();

        }

        public T GetViewModel<T>() where T : ViewModelBase
        {
            return _viewModels.OfType<T>().First<T>();
        }

        public void Load(Tournament t)
        {         
            LoadedTournament = t;
            if (t.Sessions.Count == 0) GetViewModel<SessionsViewModel>().AddSession();
            LoadedSession = t.Sessions.First(); //redo to the first not finished or the last if all are finished.

            foreach (Session ses in t.Sessions)
            {
                foreach (Section sec in ses.Sections.Where(s=>s.Movement != null))
                {
                    GetViewModel<MovementsViewModel>().Movements.Add(sec.Movement);
                }
            }

            setUpCollection(LoadedTournament.Players);
            setUpCollection(LoadedTournament.Sessions);
            setUpCollection(LoadedTournament.Pairs);
            LoadedTournament.Sessions.ToList().ForEach(s => setUpCollection(s.Sections));

            void setUpCollection<T>(ObservableCollection<T> list) where T : IndexedNamedObject
            {
                list.CollectionChanged += CollectionChanged<Player>;
                list
                     .ToList()
                    .ForEach(p => p.IndexChanged +=
                    delegate (IndexedNamedObject sender, int old, int @new)
                    { OnNumberChanged(list, sender as IndexedNamedObject, old); });
            }
        }


        public void OnNumberChanged<T>(IEnumerable<T> list, T unit, int oldId) where T : IndexedNamedObject
        {
            if (this.NumberChangeOperation) return;
            else if (unit.Number == oldId) return;
            else this.NumberChangeOperation = true;

            string s = unit.GetType().Name;

            if (list.Count(pl => pl.Number == unit.Number) > 1)
            {
                var dialog = dialogService.GetYesOrNo($"A {s} of the same number already exists, do you want to swap numbers?");
                switch (dialog)
                {
                    case DialogResult.Cancel:
                        unit.Number = oldId; 
                        break;
                    case DialogResult.Yes:
                        var unitsWithSameIndex = from p in list
                                                 where p.Number == unit.Number && p.Equals(unit) == false
                                                 select p;
                        unitsWithSameIndex.ToList()
                         .ForEach(p => p.Number = oldId);
                        break;
                    case DialogResult.No: 
                        break;
                }
            }
            this.NumberChangeOperation = false;
        }

        private void CollectionChanged<T>(object sender, NotifyCollectionChangedEventArgs e) where T : IndexedNamedObject
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                (sender as IEnumerable<T>)?.ToList().ForEach(
                    p =>
                    {
                        if (typeof(T) == typeof(Session)) (p as Session).Sections.CollectionChanged += CollectionChanged<Section>;

                        p.IndexChanged += delegate (IndexedNamedObject unit, int old, int @new)
                        {
                            OnNumberChanged(sender as IEnumerable<T>, unit as IndexedNamedObject, old);
                        };

                    });
            }
        }

    }

    
}

