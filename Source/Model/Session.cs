using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.Model
{
    public class Session {

        private int sessionNumber;
        private ObservableCollection<Section> sections;
        

        public int SessionNumber { get => sessionNumber; set => sessionNumber = value; }
        public ObservableCollection<Section> Sections {get => sections;}
        

        public Session(int number) {
            this.sessionNumber = number;
            this.sections = new ObservableCollection<Section>();
        }

        public Section addSection() {
            Section section = new Section();
            this.sections.Add(section);
            return section;

        }

        public bool removeSection() {
            throw new NotImplementedException();
        }

    }
}
