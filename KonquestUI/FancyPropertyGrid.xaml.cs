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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using com.KonquestUI.Controls;
using System.Windows.Forms.PropertyGridInternal;

namespace com.KonquestUI.Controls
{
    /// <summary>
    /// Interaction logic for FancyPropertyGrid.xaml
    /// </summary>
    /// 
    [ContentProperty(nameof(Children))]
    public partial class FancyPropertyGrid : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void onPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null) 
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler<PropertyGridItemChangedEventArgs> onPropertyItemChanged;

        public void PropertyItemChanged(object sender, PropertyGridItemChangedEventArgs e)
        {
            if(onPropertyItemChanged != null)
                onPropertyItemChanged(sender, e);

        }
        public static readonly DependencyPropertyKey ChildrenProperty = DependencyProperty.RegisterReadOnly(
               nameof(Children),  // Prior to C# 6.0, replace nameof(Children) with "Children"
               typeof(UIElementCollection),
               typeof(FancyPropertyGrid),
               new PropertyMetadata());

        public UIElementCollection Children
        {
            get { return (UIElementCollection)GetValue(ChildrenProperty.DependencyProperty); }
            private set 
            { 
                SetValue(ChildrenProperty, value); 
                onPropertyChanged(nameof(Children));
            }
        }

        private object[] selected_objects;

        [System.ComponentModel.TypeConverter((typeof(SelectedObjectConverter)))]
        [Category("Data"), DefaultValue("null"), Description("Data that you want to put into the property grid.")]
        public object SelectedObject
        {
            get
            {
                if ((object[])GetValue(SelectedObjectsProperty) == null)
                {
                    return null;
                }
                else
                {
                    return (object[])GetValue(SelectedObjectsProperty);
                }
            }
            set
            {
                if (value == null)
                {
                    SetValue(SelectedObjectsProperty, null);
                }
                else
                {
                    SetValue(SelectedObjectsProperty, new object[] { value });
                }
            }
        }

        //-- needs to handle updates to SelectedObject 
        public static readonly DependencyProperty SelectedObjectProperty = DependencyProperty.Register("SelectedObject",
            typeof(object),
            typeof(FancyPropertyGrid),
            new PropertyMetadata(null));

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object[] SelectedObjects
        {
            get
            {
                if ((object[])GetValue(SelectedObjectsProperty) == null)
                {
                    return new object[0];
                }
                return (object[])GetValue(SelectedObjectsProperty);
            }
            set
            {
                if (value == null)
                {
                    SetValue(SelectedObjectsProperty, new object[0]);
                }
                else
                {
                    SetValue(SelectedObjectsProperty, (object[])value);
                }
            }

            /*
            
                if (selected_objects[0] != null)
                {
                    PropertyInfo[] properties = selected_objects[0].GetType().GetProperties();
                    foreach (var prop in properties)
                    {
                        TextBlock tb = new TextBlock();
                        tb.Text = prop.Name;
                        tb.VerticalAlignment = VerticalAlignment.Top;
                        tb.Height = 24;
                        tb.FontSize = 12;
                        tb.Foreground = Brushes.White;
                        PART_Host.Children.Add(tb);
                        System.Diagnostics.Debug.WriteLine($"Property Name: {prop.Name}");
                    }
                }
          
                 root_grid_item = null;
				SelectItemCore (null, null); // unselect current item in the view
				if (value != null) {
					for (int i = 0; i < value.Length; i++) {
						if (value [i] == null)
							throw new ArgumentException (String.Format ("Item {0} in the objs array is null.", i));
					}
					selected_objects = value;
				} else {
					selected_objects = new object [0];
				}

				ShowEventsButton (false);
				PopulateGrid (selected_objects);
				RefreshTabs(PropertyTabScope.Component);
				if (root_grid_item != null)
					SelectItemCore (null, GetDefaultPropertyItem (root_grid_item, selected_tab));
				property_grid_view.UpdateView ();
				OnSelectedObjectsChanged (EventArgs.Empty);
         
        */
        }

        public static readonly DependencyProperty SelectedObjectsProperty = DependencyProperty.Register("SelectedObjects",
            typeof(object[]),
            typeof(FancyPropertyGrid),
            new PropertyMetadata(null, new PropertyChangedCallback(OnSelectedObjectsChanged)));


        private string _propertyname = "";
        public string PropertyName
        {
            get => _propertyname;
            set { 
                _propertyname = value; 
                onPropertyChanged(nameof(PropertyName));
            }
        }
        private string _propertydescription = "";
        public string PropertyDescription
        {
            get => _propertydescription;
            set
            {
                _propertydescription = value;
                onPropertyChanged(nameof(PropertyDescription));
            }
        }

        public FancyPropertyGrid()
        {
            InitializeComponent();
            Children = PART_Host.Children;

            if (selected_objects == null)
                selected_objects = new object[0];

            this.DataContext = this;

        }
        
        private FancyPropertyGridView gridview = null;
        private void BuildControls()
        {
            //-- here we loop through the SelectedObjects. 
            //-- And we implement our own PropertyGrid type to the data.
            //-- ColorPicker, FilePIcker, NumericValue, FlagPicker, Checkbox. 
            //-- we'll have to handle the mouse over function of each element to change the DOCComment.
            //-- We could even make the element selectable and tab indexed.
            //-- if we want to, we can create a gridsplitter. 
            if (SelectedObjects.Length == 0)
            {
                if (gridview != null)
                {
                    gridview.Clear();
                }
                if (PART_Host.Children.Count > 0)
                    PART_Host.Children.Clear();
            }

            if (SelectedObjects != null)
            {
                if (SelectedObjects.Length > 0)
                {
                    if(gridview != null)
                    {
                        gridview.Clear();
                        if(PART_Host.Children.Count > 0)
                            PART_Host.Children.Clear();

                    }

                    gridview = new FancyPropertyGridView(SelectedObjects[0]);
                    gridview.VerticalAlignment = VerticalAlignment.Stretch;
                    gridview.PropertyGrid = this;

                    PART_Host.Children.Add(gridview);
                    gridview.BuildGridViewItems();
                }
            }
        }
        public void Reload()
        {
            if (gridview != null)
            {
                gridview.Update();
                /*
                gridview.Clear();
                gridview = null;

                if (PART_Host.Children.Count > 0)
                    PART_Host.Children.Clear();
                */
            }
            //BuildControls();
        }
        private static void OnSelectedObjectsChanged(DependencyObject d,
        DependencyPropertyChangedEventArgs e)
        {
            FancyPropertyGrid propertygrid = d as FancyPropertyGrid;
            propertygrid.OnSelectedObjectsChanged(e);
        }
        private void OnSelectedObjectsChanged(DependencyPropertyChangedEventArgs e)
        {

            BuildControls();
        }
    }

    internal class SelectedObjectConverter : ReferenceConverter
    {
        public SelectedObjectConverter() : base(typeof(IComponent))
        {
        }
    }

}

