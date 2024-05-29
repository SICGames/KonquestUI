using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.KonquestUI.Controls
{
    [AttributeUsage(AttributeTargets.All)]
    public class ExpandableAttribute : Attribute
    {

        private bool expandable;

        public ExpandableAttribute(bool expandable)
        {
            this.expandable = expandable;
        }
        public bool IsExpandable
        {
            get => this.expandable;
        }
    }
}