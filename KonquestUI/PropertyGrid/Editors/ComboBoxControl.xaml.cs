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
    /// Interaction logic for ComboBoxControl.xaml
    /// </summary>
    public partial class ComboBoxControl : EditorControlsBase
    {
        public object Items
        {
            get => (object)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }
        private int selectedindex = 0;
        public int SelectedIndex
        {
            get => selectedindex;
            set => selectedindex = value;
        }
        private static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(nameof(Items), typeof(object), typeof(ComboBoxControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public ComboBoxControl() : base()
        {
            InitializeComponent();
            OldValue = SelectedIndex;
            NewValue = SelectedIndex;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            var item = (PropertyGridItem)this.Tag;
            NewValue = Enum.ToObject(item.PropertyType, cb.SelectedIndex);

        }
    }
}
