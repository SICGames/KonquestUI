﻿using System;
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

namespace com.KonquestUI.Controls
{
   
    [TemplatePart(Name = "ACTIVE_LIGHT", Type = typeof(Rectangle))]
    public class FancyNavigationButton : Button
    {
        private Rectangle _rectangle;
        private Brush _previousBrush;

        static FancyNavigationButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FancyNavigationButton), new FrameworkPropertyMetadata(typeof(FancyNavigationButton)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _rectangle = GetTemplateChild("ACTIVE_LIGHT") as Rectangle;
            if (IsActive)
                SetActive(this, true);
        }

        public static readonly DependencyProperty isActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(FancyNavigationButton),
            new PropertyMetadata(false, IsActiveChanged));
        public static readonly DependencyProperty ActiveBrushProperty = DependencyProperty.Register("ActiveBrush",
            typeof(Brush), typeof(FancyNavigationButton), new PropertyMetadata(Brushes.LightGreen));

        public static readonly DependencyProperty ActiveBackgroundBrushProperty = DependencyProperty.Register("ActiveBackgroundBrush",
            typeof(Brush), typeof(FancyNavigationButton), new PropertyMetadata(Brushes.LightGray));

        public static readonly DependencyProperty HoverBrushProperty = DependencyProperty.Register("HoverBrush",
            typeof(Brush), typeof(FancyNavigationButton), new PropertyMetadata(Brushes.LightGray));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(FancyNavigationButton), new PropertyMetadata(""));
        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource",
            typeof(BitmapSource),
            typeof(FancyNavigationButton), new PropertyMetadata(null));

        public static readonly DependencyProperty NavigationSourceProperty = DependencyProperty.Register("NavigationSource", typeof(Uri), typeof(FancyNavigationButton), new PropertyMetadata(null));

        public bool IsActive
        {
            get => (Boolean)GetValue(isActiveProperty);
            set => SetValue(isActiveProperty, value);
        }
        public Brush ActiveBrush
        {
            get => (Brush)GetValue(ActiveBrushProperty);
            set => SetValue(ActiveBrushProperty, value);
        }
        public Brush ActiveBackgroundBrush
        {
            get => (Brush)GetValue(ActiveBackgroundBrushProperty);
            set => SetValue(ActiveBackgroundBrushProperty, value);
        }

        public Brush HoverBrush
        {
            get => (Brush)GetValue(HoverBrushProperty);
            set => SetValue(HoverBrushProperty, value);
        }
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        public BitmapSource ImageSource
        {
            get => (BitmapSource)GetValue(ImageSourceProperty);
            set { SetValue(ImageSourceProperty, value); }
        }

        public Uri NavigationSource
        {
            get => (Uri)GetValue(NavigationSourceProperty);
            set => SetValue(NavigationSourceProperty, value);
        }

        private static void IsActiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var navButton = d as FancyNavigationButton;
            if (navButton == null) return;

            navButton.SetActive(d, (bool)e.NewValue);
        }
        public void SetActive(DependencyObject d, bool active)
        {

            if (_rectangle == null) return;

            _rectangle.Visibility = active ? Visibility.Visible : Visibility.Hidden;
            var navbutton = d as FancyNavigationButton;
            if (_previousBrush == null)
                _previousBrush = navbutton.Background;

            navbutton.Background = active == true ? ActiveBackgroundBrush : _previousBrush;
        }
    }
}
