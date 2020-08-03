using BridgeManager.Source.Model;
using BridgeManager.Source.Services.Dialog;
using BridgeManager.Source.ViewModel.Commands;
using BridgeManager.Source.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace BridgeManager.Source.ViewModel
{
    class SectionsViewModel : ViewModelBase {

        private IDialogService dialogService;

        public Session LoadedSession { get => MainViewModel.LoadedSession; }
        public ObservableCollection<Section> Sections { get => LoadedSession?.Sections; }

        public ICommand AddSectionCommand { get; set; }
        public ICommand RemoveSectionCommand { get; set; }
        public ICommand AssignMovementToSectionCommand { get; set; }
        public override string Header { get => Properties.Strings.sections_title; }

        public SectionsViewModel(
            MainWindowViewModel mainWindowViewModel,
            IDialogService dialogService) : base(mainWindowViewModel) {
            _view = new SectionsControl();
          

            this.dialogService = dialogService;

            this.AddSectionCommand = new DelegateCommand(() => AddSection());
            this.RemoveSectionCommand = new DelegateCommand<Section>(RemoveSection);
            this.AssignMovementToSectionCommand = new DelegateCommand<Section>(AssignMovement);
        }

        public Section AddSection() {
            Section section = new Section(LoadedSession.Sections.Count + 1);
            LoadedSession?.Sections.Add(section);
            return section;
        }

        public void RemoveSection(Section section) {
            if (section == null) {
                Console.WriteLine(Properties.Strings.sections_remove_section_no_section_selected);
                return;
            }
            else if (!LoadedSession.Sections.Remove(section)) {
                Console.WriteLine(Properties.Strings.sections_remove_section_couldnt_remove);
            }
            else {
                foreach (Section s in from s in LoadedSession.Sections
                                      where s.Number > section.Number
                                      select s) {
                    s.Number -= 1;
                }
            }
        }

        public void AssignMovement(Section toSection) {
            if (toSection == null) {
                Console.WriteLine(Properties.Strings.sections_assign_movement_no_section_selected);
                return;
            }
            Movement movement = dialogService.SelectMovement(MainViewModel.GetViewModel<MovementsViewModel>().Movements);
            toSection.Movement = movement;
        }
    }
}
