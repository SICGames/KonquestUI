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
using com.KonquestUI.Controls;

namespace com.KonquestUI.PropertyGrid.Editors
{
    /// <summary>
    /// Interaction logic for NumericValueControl.xaml
    /// </summary>
    public partial class NumericValueControl : EditorControlsBase
    {
        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set
            {
                SetValue(ValueProperty, value);
            }
        }
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(NumericValueControl), new PropertyMetadata(0d));

        public NumericValueControl() : base()
        {
            InitializeComponent();
            OldValue = Value;
            NewValue = Value;
        }

        private void FancyNumericValue_ValueChanged(object sender, RoutedEventArgs e)
        {
            var fnv = (FancyNumericValue)sender;
            base.OldValue = Value;

            if (fnv != null)
            {
                Value = fnv.Value;
                base.NewValue = fnv.Value;
            }
        }
    }
}
