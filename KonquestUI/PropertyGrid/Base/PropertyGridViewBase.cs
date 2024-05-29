using com.KonquestUI.Controls;
using com.KonquestUI.PropertyGridView.Extended;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using com.KonquestUI.PropertyGrid.Editors;

namespace com.KonquestUI.Base
{
    public class PropertyGridViewBase : UserControl
    {
        public PropertyGridViewBase()
        {
        }

        public PropertyGridItemCollection GridViewCollection { get; set; }
        public PropertyGridItem[] entries = null;
        public PropertyGridItem currentPropertyGridItem = null;
        public object UserData { get; set; }
        public FancyPropertyGrid PropertyGrid { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public Grid mainGrid { get; set; }

        public PropertyInfo[] GetProperties(object source)
        {
            return source.GetType().GetProperties();
        }
        public static UIElement GetGridElement(Grid g, int r, int c)
        {
            for (int i = 0; i < g.Children.Count; i++)
            {
                UIElement e = g.Children[i];
                if (Grid.GetRow(e) == r && Grid.GetColumn(e) == c)
                    return e;
            }
            return null;
        }

        //-- expandable should be the parent. Expanded should be the children.
        private void HandleBrowseability(FrameworkElement element, PropertyGridItem item, int index)
        {
            Grid grid = (Grid)element;
            //-- update browseability
            if (item.Browseable != null)
            {
                var visible = item.Browseable.Browsable;
                ((RowDefinitionExtended)grid.RowDefinitions[index]).Visible = visible;
            }
            //--- just in case some controls hide themselves because Browseable is null.

        }
        private void HandleExpandability(FrameworkElement element, PropertyGridItem item, int index)
        {
            Grid grid = (Grid)element;
            
            LabelControl lc = null;
            if (item.ExpandableAttribute != null)
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    lc = (LabelControl)GetGridElement(grid, index, 0);
                    if (lc != null)
                    {
                        lc.IsExpandable = item.ExpandableAttribute.IsExpandable;
                        //-- we have to fetch a head to see next row is visible or not. 
                        var isVisible = ((RowDefinitionExtended)grid.RowDefinitions[index + 1]).Visible;
                        if (isVisible)
                        {
                            lc.IsExpanded = true;
                        }
                        else
                        {
                            lc.IsExpanded = false;
                        }
                    }
                }));
            }
        }
        public void Refresh()
        {

            Grid grid = (Grid)mainGrid;
            var index = 0;

            foreach (PropertyGridItem item in GridViewCollection)
            {
                HandleBrowseability(grid, item, index);
                HandleExpandability(grid, item, index);
                index++;
            }
        }

        public void Update()
        {
            if (UserData == null)
                return;
            if (GridViewCollection.Count < 1)
                return;

            //--- go through and update the values of PropertyGrid.SelectedObject.
            
            PropertyInfo[] properties = GetProperties(PropertyGrid.SelectedObjects[0]);
            if (properties.Length > 0)
            {

                foreach (var property in properties)
                {
                    var griditem = GridViewCollection[property.Name];
                    if (property.PropertyType == typeof(System.Single))
                    {
                        property.SetValue(PropertyGrid.SelectedObjects[0], Convert.ToSingle(griditem.Value));
                        PropertyGrid.PropertyItemChanged(PropertyGrid.SelectedObjects[0], new PropertyGridItemChangedEventArgs(griditem.Header, griditem.Value));
                    }
                    else if (property.PropertyType == typeof(byte))
                    {
                        property.SetValue(PropertyGrid.SelectedObjects[0], Convert.ToByte(griditem.Value));
                        PropertyGrid.PropertyItemChanged(PropertyGrid.SelectedObjects[0], new PropertyGridItemChangedEventArgs(griditem.Header, griditem.Value));
                    }
                    else if (property.PropertyType == typeof(int))
                    {
                        property.SetValue(PropertyGrid.SelectedObjects[0], Convert.ToInt32(griditem.Value));
                        PropertyGrid.PropertyItemChanged(PropertyGrid.SelectedObjects[0], new PropertyGridItemChangedEventArgs(griditem.Header, griditem.Value));
                    }
                    else if(property.PropertyType == typeof(uint))
                    {
                        property.SetValue(PropertyGrid.SelectedObjects[0], Convert.ToUInt32(griditem.Value));
                        PropertyGrid.PropertyItemChanged(PropertyGrid.SelectedObjects[0], new PropertyGridItemChangedEventArgs(griditem.Header, griditem.Value));
                    }
                    else
                    {
                        property.SetValue(PropertyGrid.SelectedObjects[0], griditem.Value);
                        PropertyGrid.PropertyItemChanged(PropertyGrid.SelectedObjects[0], new PropertyGridItemChangedEventArgs(griditem.Header, griditem.Value));
                    }
                }
                
                Refresh();
            }
        }

    }
}
