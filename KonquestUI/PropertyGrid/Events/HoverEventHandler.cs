using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace com.KonquestUI.Controls
{
    public class HoverEventArgs : RoutedEventArgs
    {
        private readonly string _PropertyName;
        private readonly string _PropertyDescription;
        public string PropertyName
        {
            get => _PropertyName;
        }
        public string PropertyDescription
        {
            get => _PropertyDescription;
        }

        public HoverEventArgs(RoutedEvent routedEvent, string propertyname, string propertydescription) : base(routedEvent)
        {
            this._PropertyName = propertyname;
            this._PropertyDescription = propertydescription;
        }
    }
}
