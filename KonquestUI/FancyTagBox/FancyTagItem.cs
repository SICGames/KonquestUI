﻿using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class FancyTagItem : Button
    {
        TextBlock textblock { get; set; }

        static FancyTagItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FancyTagItem), new FrameworkPropertyMetadata(typeof(FancyTagItem)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            textblock = base.Template.FindName("TEXT_CONTENT", this) as TextBlock;
            Border border = base.Template.FindName("MAIN_BORDER", this) as Border;
            border.PreviewMouseDown += Border_PreviewMouseDown;
        }

        private static T FindParent<T>(DependencyObject child)
      where T : DependencyObject
        {
            if (child == null) return null;

            T foundParent = null;
            var currentParent = VisualTreeHelper.GetParent(child);

            do
            {
                var frameworkElement = currentParent as FrameworkElement;
                if (frameworkElement is T)
                {
                    foundParent = (T)currentParent;
                    break;
                }

                currentParent = VisualTreeHelper.GetParent(currentParent);

            } while (currentParent != null);

            return foundParent;
        }

        private void Border_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            //-- need to communicate with TagBox and have it remove this
            var ftb = FindParent<FancyTagBox>(this);
            ftb.RemoveSelectedTagItem(this.Label);
        }

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(FancyTagItem), new PropertyMetadata("", LabelTextChanged));
        public static readonly DependencyProperty CornersPropery = DependencyProperty.Register("Corners", typeof(CornerRadius), typeof(FancyTagItem),
            new PropertyMetadata(new CornerRadius(0, 0, 0, 0)));

        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public CornerRadius Corners
        {
            get => (CornerRadius)GetValue(CornersPropery);
            set => SetValue(CornersPropery, value);
        }

        private static void LabelTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d == null) return;
            var fbi = (FancyTagItem)d;
            fbi.onLabelChanged(d, e.NewValue.ToString());
        }
        public void onLabelChanged(object sender, string text)
        {
            if (textblock == null)
                return;

            FormattedText formattedText = new FormattedText(text, CultureInfo.GetCultureInfo("en-us"),
                              FlowDirection.LeftToRight,
                              new Typeface(this.textblock.FontFamily, this.textblock.FontStyle, this.textblock.FontWeight, this.textblock.FontStretch),
                              this.textblock.FontSize,
                              Brushes.Black,
                              new NumberSubstitution(),
                              VisualTreeHelper.GetDpi(textblock).PixelsPerDip);

            formattedText.MaxTextWidth = 300;
            formattedText.MaxTextHeight = 240;
            var size = new Size(formattedText.Width, formattedText.Height);
            this.SetValue(WidthProperty, size.Width + 32);
        }
    }
}
