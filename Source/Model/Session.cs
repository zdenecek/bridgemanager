﻿using BridgeManager.Source.Component;
using BridgeManager.Source.Model.Scoring;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.Model
{

    public class Session : IndexedNamedObject {

        private string _databaseFilepath;

        public ObservableCollection<Section> Sections { get; set; }

        public string DatabaseFilepath { get => _databaseFilepath; set { _databaseFilepath = value; OnPropertyChanged(); } }

        public ProgressState ProgressState { get; set; }

        public ObservableCollection<Result> Results { get; set; }
        public ObservableCollection<Result> IntermediateResults { get; set; }

        public ObservableCollection<Score> IntermediateScores { get; set; }
        public ObservableCollection<Score> FinalScores { get; set; }
        /// <summary>
        /// First index denotes the section
        /// Second and first indeces denote the unit number
        /// </summary>
        public Score[][,] CrossTable { get; set; }

        public Session(int number) : base(number) {
            this.Name = "Section " + Number;
            this.Sections = new ObservableCollection<Section>();

            this.Results = new ObservableCollection<Result>();
            this.IntermediateResults = new ObservableCollection<Result>();
            this.IntermediateScores = new ObservableCollection<Score>();
            this.FinalScores = new ObservableCollection<Score>();
        }

        public Session() : this(number: 0)
        { }

    }
}
