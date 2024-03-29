﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeManager.Source.Component
    {
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null) {
         //   Console.WriteLine($"Observable object changed, property name {propertyName} , source {this.ToString()}");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        
        }

    }
}
