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
using System.Windows.Forms;

namespace com.KonquestUI.PropertyGrid.Editors
{
    /// <summary>
    /// Interaction logic for FilePickerControl.xaml
    /// </summary>
    public partial class FilePickerControl : EditorControlsBase
    {
        public string File
        {
            get => (string)GetValue(FileProperty);
            set => SetValue(FileProperty, value);
        }
        public static readonly DependencyProperty FileProperty = DependencyProperty.Register("File", typeof(String), typeof(FilePickerControl), new PropertyMetadata(""));

        public FilePickerControl() : base()
        {
            InitializeComponent();
            OldValue = File;
            NewValue = File;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            //-- mostly textures. 
            ofd.Filter = "All Images | *.jpg; *.png; *.dds; *.tiff; *.bmp; *.tga";
            ofd.Title = "Choose File...";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                File = ofd.FileName;
                NewValue = File;
            }
        }
    }
}
