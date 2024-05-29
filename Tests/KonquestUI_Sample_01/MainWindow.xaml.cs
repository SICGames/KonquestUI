using com.KonquestUI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KonquestUI_Sample_01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        TestClass TestClass { get; set; }

        private string pGreeting;
        public string Greeting
        {
            get => pGreeting;
            set
            {
                pGreeting = value;
                onPropertyChanged(nameof(Greeting));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            TestClass = new TestClass();
            PropertyGrid.onPropertyItemChanged += PropertyGrid_onPropertyItemChanged;
            this.DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void onPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));  
        }

        private void PropertyGrid_onPropertyItemChanged(object sender, com.KonquestUI.Controls.PropertyGridItemChangedEventArgs e)
        {
            var source = sender as TestClass;
            Greeting = $"{source.Name} is {source.Age.ToString()} and loves to {source.Description}";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PropertyGrid.SelectedObject = TestClass;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
