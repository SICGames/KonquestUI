using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.KonquestUI.Controls
{
    public class PropertyGridItemChangedEventArgs : EventArgs
    {
        private readonly string _itemname;
        private readonly object _itemvalue;
        public PropertyGridItemChangedEventArgs(string ItemName, Object ItemValue)
        {
            this._itemname = ItemName;
            this._itemvalue = ItemValue;
        }

        public virtual string ItemName
        {
            get => _itemname;
        }
        public virtual object ItemValue
        {
            get => _itemvalue;
        }
    }
}
