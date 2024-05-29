using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using com.KonquestUI.Base;
using com.KonquestUI.PropertyGrid.Editors;
using com.KonquestUI.PropertyGridView.Extended;

namespace com.KonquestUI.Controls
{
    /// <summary>
    /// Interaction logic for FancyPropertyGridView.xaml
    /// </summary>
    public partial class FancyPropertyGridView : PropertyGridViewBase
    {
        private static readonly DependencyProperty splitterVisibleProperty = DependencyProperty.Register(nameof(SplitterVisible), typeof(bool), typeof(FancyPropertyGridView), new UIPropertyMetadata(false));
        public bool SplitterVisible
        {
            get => (bool)GetValue(splitterVisibleProperty);
            set => SetValue(splitterVisibleProperty, value);
        }

        public static readonly DependencyPropertyKey ChildrenProperty = DependencyProperty.RegisterReadOnly(
            nameof(Children),  // Prior to C# 6.0, replace nameof(Children) with "Children"
            typeof(UIElementCollection),
            typeof(FancyPropertyGridView),
            new PropertyMetadata());

        public UIElementCollection Children
        {
            get { return (UIElementCollection)GetValue(ChildrenProperty.DependencyProperty); }
            private set { SetValue(ChildrenProperty, value); }
        }


        public void Clear()
        {
            mainGrid.Children.Clear();
            grid.Children.Clear();
            UserData = null;
        }

        public void Populate()
        {
            if (UserData == null)
                return;

            PropertyInfo[] properties = GetProperties(UserData);
            entries = new PropertyGridItem[properties.Length];
            var entryIndex = 0;

            foreach (var prop in properties)
            {
                AttributeCollection attributecollection = TypeDescriptor.GetProperties(UserData)[prop.Name].Attributes;
                DescriptionAttribute descriptionAttribute = (DescriptionAttribute)attributecollection[typeof(DescriptionAttribute)];
                DisplayNameAttribute displayNameAttribute = (DisplayNameAttribute)attributecollection[typeof(DisplayNameAttribute)];
                EditorAttribute editorAttribute = (EditorAttribute)attributecollection[typeof(EditorAttribute)];
                BrowsableAttribute browsableAttribute = (BrowsableAttribute)attributecollection[typeof(BrowsableAttribute)];
                CategoryAttribute categoryAttribute = (CategoryAttribute)attributecollection[typeof(CategoryAttribute)];
                ExpandableAttribute expandableAttribute = (ExpandableAttribute)attributecollection[typeof(ExpandableAttribute)];
                PropertyGridItem entry = new PropertyGridItem();

                entry.Header = prop.Name;
                entry.Value = prop.GetValue(UserData);
                entry.DisplayName = displayNameAttribute;
                entry.Browseable = browsableAttribute;
                entry.Catagory = categoryAttribute;
                entry.Description = descriptionAttribute;
                entry.PropertyType = prop.PropertyType;
                entry.EditorAttribute = editorAttribute;
                entry.ExpandableAttribute = expandableAttribute;

                entries[entryIndex] = entry;
                entryIndex++;
            }
            GridViewCollection = new PropertyGridItemCollection(entries);
            entries = null;
        }
        private void InitializeGridView()
        {
            //-- build basics.
            //-- 3 columns. 0 - left side. 1 - splitter. 2 - right side. 
            mainGrid = new Grid();
            mainGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            mainGrid.VerticalAlignment = VerticalAlignment.Stretch;
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinition cd = new ColumnDefinition();
            cd.Width = new GridLength(5);
            mainGrid.ColumnDefinitions.Add(cd);
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            mainGrid.RowDefinitions.Add(new RowDefinitionExtended());
            grid.Children.Add(mainGrid);

        }

        //-- Experimental
        private void AddRowItem(UIElement element, int rowIndex, int colIndex, bool visible)
        {
            //RowDefinition rd = new RowDefinition();
            RowDefinitionExtended rd = new RowDefinitionExtended();
            //rd.Height = new GridLength(18);
            mainGrid.RowDefinitions.Add(rd);
            mainGrid.Children.Add(element);
            Grid.SetRow(element, rowIndex);
            Grid.SetColumn(element, colIndex);
            ((RowDefinitionExtended)mainGrid.RowDefinitions[rowIndex]).Visible = visible;
        }
        public void BuildGridViewItems()
        {
            Populate();
            InitializeGridView();
            if (GridViewCollection.Count > 0)
            {
                var gridRowCount = 0;
                mainGrid.ShowGridLines = false;

                foreach (PropertyGridItem gi in GridViewCollection)
                {
                    var fileBrowserRequested = false;
                    var friendlyName = String.IsNullOrEmpty(gi.DisplayName.DisplayName) == false ? gi.DisplayName.DisplayName : gi.Header;
                    if (gi.EditorAttribute != null)
                    {
                        if (gi.EditorAttribute.EditorTypeName.Contains("FileNameEditor"))
                            fileBrowserRequested = true;
                    }
                    var description = String.IsNullOrEmpty(gi.Description.Description) == false ? gi.Description.Description : "";

                    var browseable = false;
                    if (gi.Browseable != null)
                        browseable = gi.Browseable.Browsable;
                    else
                        browseable = true;

                    var propertyType = gi.PropertyType;

                    //-- check to see if it's expandable

                    //-- fill in the label controls.
                    LabelControl lc = new LabelControl();
                    lc.Name = $"Label{gridRowCount}";
                    lc.Text = friendlyName;
                    lc.PropertyName = gi.Header;
                    lc.PropertyDescription = description;
                    lc.Height = 16;
                    lc.Margin = new Thickness(0, 1, 0, 1);
                    lc.Hover += Lc_Hover;

                    bool expandable = gi.ExpandableAttribute != null && gi.ExpandableAttribute.IsExpandable;
                    lc.IsExpandable = expandable;
                    lc.IsExpanded = false;

                    AddRowItem(lc, gridRowCount, 0, browseable);

                    GridSplitter gs = new GridSplitter();
                    gs.VerticalAlignment = VerticalAlignment.Stretch;
                    gs.HorizontalAlignment = HorizontalAlignment.Center;
                    gs.Width = 4;
                    gs.Height = grid.Height;
                    gs.Background = new SolidColorBrush(Color.FromArgb(255, 34, 34, 34));
                    gs.ResizeBehavior = GridResizeBehavior.PreviousAndNext;
                    gs.ResizeDirection = GridResizeDirection.Columns;
                    AddRowItem(gs, gridRowCount, 1, browseable);
                    
                    //-- let's put in the value controls.
                    if (gi.PropertyType == typeof(bool))
                    {
                        CheckBoxControl cb = new CheckBoxControl();
                        cb.Tag = gi;
                        cb.isChecked = Convert.ToBoolean(gi.Value);
                        cb.HorizontalAlignment = HorizontalAlignment.Stretch;
                        cb.Height = 16;
                        cb.Margin = new Thickness(0, 2, 0, 2);
                        cb.ValueChanged += Cb_ValueChanged;
                        AddRowItem(cb, gridRowCount, 2, browseable);
                    }
                    else if (gi.PropertyType == typeof(Single) || gi.PropertyType == typeof(byte) || gi.PropertyType == typeof(uint))
                    {
                        NumericValueControl nvc = new NumericValueControl();
                        nvc.Tag = gi;
                        nvc.Value = Convert.ToDouble(gi.Value);
                        nvc.HorizontalAlignment = HorizontalAlignment.Stretch;
                        nvc.Height = 16;
                        nvc.Margin = new Thickness(0, 2, 0, 2);
                        nvc.ValueChanged += Nvc_ValueChanged;
                        AddRowItem(nvc, gridRowCount, 2, browseable);
                    }
                    
                    else if (gi.PropertyType == typeof(String))
                    {
                        if (fileBrowserRequested)
                        {
                            FilePickerControl fpc = new FilePickerControl();
                            var file = gi.Value == null ? "" : gi.Value.ToString();
                            fpc.Tag = gi;
                            fpc.File = file;
                            fpc.HorizontalAlignment = HorizontalAlignment.Stretch;
                            fpc.Height = 16;
                            fpc.Margin = new Thickness(2, 2, 2, 2);
                            fpc.ValueChanged += Fpc_ValueChanged;
                            AddRowItem(fpc, gridRowCount, 2, browseable);
                            fileBrowserRequested = false;
                        }
                        else
                        {
                            TextBox textBox = new TextBox();
                            textBox.Tag = gi;
                            textBox.Text = gi.Value.ToString();
                            textBox.HorizontalAlignment = HorizontalAlignment.Stretch;
                            textBox.Height = 16;
                            textBox.Margin = new Thickness(2, 2, 2, 2);
                            textBox.TextChanged += TextBox_TextChanged;
                            AddRowItem(textBox, gridRowCount, 2, browseable);
                        }
                    }
                   else if (gi.PropertyType == typeof(Color))
                    {
                        var color = gi.Value;
                        ColorPickerControl cpc = new ColorPickerControl();
                        cpc.Tag = gi;
                        cpc.ColorValue = (System.Windows.Media.Color)color;
                        cpc.HorizontalAlignment = HorizontalAlignment.Stretch;
                        cpc.Height = 16;
                        cpc.Margin = new Thickness(0, 2, 0, 2);
                        cpc.ValueChanged += Cpc_ValueChanged;
                        AddRowItem(cpc, gridRowCount, 2, browseable);
                    }
                  else if (gi.PropertyType.IsEnum)
                    {
                        if (gi.PropertyType.Name == "ALPHA_BLEND_MODE")
                        {
                            ComboBoxControl cbc = new ComboBoxControl();
                            cbc.Tag = gi;
                            cbc.HorizontalAlignment = HorizontalAlignment.Stretch;
                            cbc.Height = 16;
                            cbc.FontSize = 8;
                            cbc.Margin = new Thickness(0, 2, 0, 2);
                            var items = Enum.GetNames(gi.PropertyType);
                            cbc.Items = items;
                            cbc.SelectedIndex = (int)gi.Value;
                            cbc.ValueChanged += Cbc_ValueChanged;
                            AddRowItem(cbc, gridRowCount, 2, browseable);
                        }
                        if (gi.PropertyType.Name == "MASK_WRITE_FLAGS")
                        {
                            CustomComboBoxControl customComboBoxControl = new CustomComboBoxControl();
                            customComboBoxControl.Tag = gi;
                            customComboBoxControl.HorizontalAlignment = HorizontalAlignment.Stretch;
                            customComboBoxControl.Height = 16;
                            customComboBoxControl.FontSize = 8;
                            customComboBoxControl.Margin = new Thickness(0, 2, 0, 2);
                            customComboBoxControl.Value = (int)gi.Value;

                            var items = Enum.GetNames(gi.PropertyType);
                            var itemValues = Enum.GetValues(gi.PropertyType);
                            var underlyingValueType = Enum.GetUnderlyingType(gi.PropertyType);

                            var itemsDict = new Dictionary<string, object>();
                            var selectedItemsDict = new Dictionary<string, object>();

                            for (int i = 0; i < items.Length; i++)
                            {
                                var item = items[i];
                                var value = itemValues.GetValue(i);
                                var underlyingValue = Convert.ChangeType(value, underlyingValueType);
                                var flagValue = underlyingValue;

                                itemsDict.Add(item.ToString(), flagValue);

                                int flag = customComboBoxControl.Value;
                                if ((flag & (1 << i)) != 0)
                                {
                                    selectedItemsDict.Add(item.ToString(), flagValue);
                                }
                            }

                            customComboBoxControl.ItemsSource = itemsDict;
                            customComboBoxControl.SelectedItems = selectedItemsDict;
                            customComboBoxControl.ValueChanged += CustomComboBoxControl_ValueChanged;
                            AddRowItem(customComboBoxControl, gridRowCount, 2, browseable);
                        }
                    }
                    else
                    {

                    }
                    gridRowCount++;
                }
                Update();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var source = (TextBox)e.OriginalSource;
            
            var gridItem = (PropertyGridItem)source.Tag;
            var gridname = gridItem.Header;
            GridViewCollection[gridname].Value = source.Text;
            Update();
        }

        private void CustomComboBoxControl_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            CustomComboBoxControl ccbc = (CustomComboBoxControl)sender;
            var griditem = (PropertyGridItem)ccbc.Tag;
            var gridname = griditem.Header;
            GridViewCollection[gridname].Value = e.NewValue;
            Update();
        }

        private void Cbc_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            ComboBoxControl cbc = (ComboBoxControl)sender;
            var griditem = (PropertyGridItem)cbc.Tag;
            var gridname = griditem.Header;
            GridViewCollection[gridname].Value = e.NewValue;

            Update();
        }

        private void Cpc_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            ColorPickerControl cp = (ColorPickerControl)sender;
            var griditem = (PropertyGridItem)cp.Tag;
            var gridName = griditem.Header;
            GridViewCollection[gridName].Value = e.NewValue;
            Update();
        }

        private void Fpc_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            FilePickerControl fp = (FilePickerControl)sender;
            var griditem = (PropertyGridItem)fp.Tag;
            var gridName = griditem.Header;
            GridViewCollection[gridName].Value = e.NewValue;
            Update();
        }

        private void Nvc_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            NumericValueControl nv = (NumericValueControl)sender;
            var griditem = (PropertyGridItem)nv.Tag;
            var gridName = griditem.Header;
            GridViewCollection[gridName].Value = e.NewValue;

            Update();
        }

        private void Cb_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            CheckBoxControl cbc = (CheckBoxControl)sender;
            var griditem = (PropertyGridItem)cbc.Tag;
            var gridName = griditem.Header;
            GridViewCollection[gridName].Value = e.NewValue;
            Update();
        }

        private void Lc_Hover(object sender, HoverEventArgs e)
        {
            if (PropertyGrid != null)
            {
                PropertyGrid.PropertyName = e.PropertyName;
                PropertyGrid.PropertyDescription = e.PropertyDescription;
            }
        }

        public FancyPropertyGridView(object userdata) : base()
        {
            InitializeComponent();
            this.UserData = userdata;
        }
    }
}
