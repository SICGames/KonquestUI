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
using System.Windows.Shapes;

namespace com.KonquestUI.PropertyGrid.Dialogs
{
    /// <summary>
    /// Interaction logic for ColorPickerDialog.xaml
    /// </summary>
    public partial class ColorPickerDialog : Window
    {
        public Color SelectedColor
        {
            get => ColorBox.SelectedColor;
        }

        public ColorPickerDialog()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {

        }

        private void ConfirmButton1_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;

        }

        private void CancelButton1_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
