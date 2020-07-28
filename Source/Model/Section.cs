using BridgeManager.Source.Component;
using BridgeManager.Source.Model.Scoring;
using BridgeManager.Source.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace BridgeManager.Source.Model
{
    public class Section : IndexedNamedObject {

        private List<Tuple<int, int>> playerMatrix;
        private Movement movement;
        private int boardsShift;
        private object playersShift;

        public Movement Movement { get => movement; set { movement = value; OnPropertyChanged(); } }

        //@Todo
    
        public List<Tuple<int,int>> PlayerMatrix { get => playerMatrix; set { playerMatrix = value; OnPropertyChanged(); } }
        public int BoardsShift { get => boardsShift; set => boardsShift = value; }
        public object PlayersShift { get => playersShift; set => playersShift = value; }

        public Section(int number) : base(number) {
            this.playerMatrix = new List<Tuple<int, int>>();
            Name = TextualTools.GetLetter(number);
            this.BoardsShift = 0;
        }

        public Section() : this(0) { }
    }
}
