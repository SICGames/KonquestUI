using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.KonquestUI.Controls
{
    public class PropertyGridItem
    {
        public CategoryAttribute Catagory { get; set; }
        public DescriptionAttribute Description { get; set; }
        public DisplayNameAttribute DisplayName { get; set; }
        public BrowsableAttribute Browseable { get; set; }
        public String Header { get; set; }
        public object Value { get; set; }
        public Type PropertyType { get; set; }
        public EditorAttribute EditorAttribute { get; set; }
        public PropertyGridItemCollection GridItems { get; set; }
        public ExpandableAttribute ExpandableAttribute { get; set; }

        public PropertyGridItem()
        {

        }
    }
}
