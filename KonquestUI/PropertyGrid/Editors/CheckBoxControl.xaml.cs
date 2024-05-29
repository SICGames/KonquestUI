using System;
using System.Collections.Generic;
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
using com.KonquestUI.Base.Editors;

namespace com.KonquestUI.PropertyGrid.Editors
{
    /// <summary>
    /// Interaction logic for CheckBoxControl.xaml
    /// </summary>
    public partial class CheckBoxControl : EditorControlsBase
    {
        public static readonly DependencyProperty isCheckedProperty = DependencyProperty.Register("isChecked", typeof(bool), typeof(CheckBoxControl), new PropertyMetadata(false, new PropertyChangedCallback(onIsCheckedChanged)));
        public bool isChecked
        {
            get => (bool)GetValue(isCheckedProperty);
            set
            {
                SetValue(isCheckedProperty, value);
            }
        }

        public CheckBoxControl() : base()
        {
            InitializeComponent();
            OldValue = isChecked;
        }
        private static void onIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CheckBoxControl cbc = (CheckBoxControl)d;
            cbc.NewValue = cbc.isChecked;
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }
    }
}
