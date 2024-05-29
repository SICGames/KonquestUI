using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace com.KonquestUI.Controls
{
    public class ValueChangedEventArgs : RoutedEventArgs
    {
        private readonly object _oldvalue;
        private readonly object _newvalue;

        public object OldValue
        {
            get => _oldvalue;
        }
        public object NewValue
        {
            get => _newvalue;
        }

        public ValueChangedEventArgs(RoutedEvent routedEvent, object oldvalue, object newvalue) : base(routedEvent)
        {
            this._oldvalue = oldvalue;
            this._newvalue = newvalue;
        }
    }
}
