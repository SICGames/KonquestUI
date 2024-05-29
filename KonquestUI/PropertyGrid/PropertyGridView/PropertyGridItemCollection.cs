using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.KonquestUI.Controls
{
    public class PropertyGridItemCollection : ICollection
    {
        internal PropertyGridItem[] entries;

        public PropertyGridItemCollection(PropertyGridItem[] entries)
        {
            if (entries == null)
                this.entries = new PropertyGridItem[0];

            this.entries = entries;
        }
        public int Count
        {
            get => this.entries.Length;
        }
        object ICollection.SyncRoot
        {
            get => this;
        }
        bool ICollection.IsSynchronized
        {
            get => false;
        }
        public PropertyGridItem this[int index]
        {
            get => this.entries[index];
        }
        public PropertyGridItem this[string Header]
        {
            get
            {
                foreach (PropertyGridItem g in entries)
                {
                    if (g.Header == Header)
                        return g;
                }
                return null;
            }
        }
        void ICollection.CopyTo(Array array, int index)
        {
            if (entries.Length > 0)
            {
                System.Array.Copy(entries, 0, array, index, entries.Length);
            }
        }
        public IEnumerator GetEnumerator()
        {
            return entries.GetEnumerator();
        }
    }
}
