using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace com.KonquestUI
{
    public class NotifyableObject : INotifyPropertyChanged
    {
        [field: NonSerialized] public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void RaisePropertyChanged(string property)
        {
            if (property != null) PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
