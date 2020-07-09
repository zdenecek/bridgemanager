using BridgeManager.Source.Component;
using BridgeManager.Source.Model.Scoring;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace BridgeManager.Source.Model
{
    public class Section : IndexedObject {

        private List<Tuple<int, int>> playerMatrix;
        private Movement movement;
        private int boardsShift;
        private object playersShift;

        public Movement Movement { get => movement; set { movement = value; OnPropertyChanged(); } }

        //@Todo
        [XmlIgnore]
        public List<Tuple<int,int>> PlayerMatrix { get => playerMatrix; set { playerMatrix = value; OnPropertyChanged(); } }
        public int BoardsShift { get => boardsShift; set => boardsShift = value; }
        public object PlayersShift { get => playersShift; set => playersShift = value; }

        public Section(int number) : base(number) {
            this.playerMatrix = new List<Tuple<int, int>>();
            this._name = Strings.GetLetter(number);
            this.BoardsShift = 0;
        }

        private Section() : this(0) { }
    }
}
