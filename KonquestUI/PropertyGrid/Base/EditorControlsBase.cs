using com.KonquestUI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace com.KonquestUI.Base.Editors
{
    public class EditorControlsBase : UserControl, INotifyPropertyChanged
    {
        private object oldvalue = null;
        private object newvalue = null;
        public object OldValue
        {
            get => oldvalue;
            set => oldvalue = value;
        }

        public object NewValue
        {
            get => newvalue;
            set
            {
                newvalue = value;

                if (oldvalue != newvalue)
                {
                    oldvalue = newvalue;
                    ValueChangedEventArgs args = new ValueChangedEventArgs(ValueChangedEvent, oldvalue, newvalue);
                    RaiseEvent(args);
                }
            }
        }

        private object parentgridview = null;
        public Object ParentGridView
        {
            get => parentgridview;
            set => parentgridview = value;
        }

        public delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs e);

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(ValueChangedEventHandler), typeof(EditorControlsBase));
        public event ValueChangedEventHandler ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        public delegate void HoverEventHandler(object sender, HoverEventArgs e);
        public static readonly RoutedEvent HoverEvent = EventManager.RegisterRoutedEvent("Hover", RoutingStrategy.Bubble, typeof(HoverEventHandler), typeof(EditorControlsBase));
        public event HoverEventHandler Hover
        {
            add { AddHandler(HoverEvent, value); }
            remove { RemoveHandler(HoverEvent, value); }
        }

        public EditorControlsBase()
        {
            oldvalue = null;
            newvalue = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
