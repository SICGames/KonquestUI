using System.Windows;
using com.KonquestUI.Base;
using com.KonquestUI;

namespace com.KonquestUI.Controls
{
    /// <summary>
    /// Interaction logic for SimpleColorPicker.xaml
    /// </summary>
    public partial class SimpleColorPicker : PickerControlBase
    {
        public static DependencyProperty PickerTypeProperty
            = DependencyProperty.Register(nameof(PickerType), typeof(PickerType), typeof(SimpleColorPicker),
                new PropertyMetadata(PickerType.HSV));

        public static readonly DependencyProperty SmallChangeProperty =
            DependencyProperty.Register(nameof(SmallChange), typeof(double), typeof(SimpleColorPicker),
                new PropertyMetadata(1.0));

        public PickerType PickerType
        {
            get => (PickerType)GetValue(PickerTypeProperty);
            set => SetValue(PickerTypeProperty, value);
        }

        public double SmallChange
        {
            get => (double)GetValue(SmallChangeProperty);
            set => SetValue(SmallChangeProperty, value);
        }


        public SimpleColorPicker() : base()
        {
            InitializeComponent();
        }
    }
}
