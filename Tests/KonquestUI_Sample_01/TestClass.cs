using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace KonquestUI_Sample_01
{
    public class TestClass : INotifyPropertyChanged
    {
        private string pName;
        private uint pAge;
        private string pDescription;

        [DisplayName("Name Of The Person"), Description(""), Browsable(true)]
        public string Name
        {
            get { return pName; }
            set
            {
                pName = value;
                onPropertyChanged(nameof(Name));
            }
        }

        [DisplayName("Age Of The Person"), Description(""), Browsable(true)]
        public uint Age
        {
            get { return pAge; }
            set
            {
                pAge = value;
                onPropertyChanged(nameof(Age));
            }
        }
        [DisplayName("What do they look like"), Description(""), Browsable(true)]
        public string Description
        {
            get => pDescription;
            set
            {
                pDescription = value;
                onPropertyChanged(nameof(Description));
            }
        }
        
        public TestClass() 
        {
            Name = "Jessica Stone";
            Description = "Ambitious";
            Age = 23;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void onPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
