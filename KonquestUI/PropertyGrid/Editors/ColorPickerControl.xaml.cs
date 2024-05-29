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
    /// Interaction logic for ColorPickerControl.xaml
    /// </summary>
    public partial class ColorPickerControl : EditorControlsBase
    {
        public Color ColorValue
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("ColorValue", typeof(Color), typeof(ColorPickerControl), new PropertyMetadata(Color.FromArgb(1, 1, 1, 1)));

        public ColorPickerControl() : base()
        {
            InitializeComponent();
            this.DataContext = this;
            OldValue = ColorProperty;
        }
        private void ColorBox01_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                /*
                FancyColorPickerDialog cpd = new ColorPickerDialog();

                if (cpd.ShowDialog() == true)
                {
                    ColorValue = cpd.SelectedColor;
                    NewValue = ColorValue;
                }
                */
            }
        }
    }
}
