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

namespace com.KonquestUI.Controls
{
    public class FancyFilePicker : Button
    {
        private TextBox pTextBox;

        static FancyFilePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FancyFilePicker), new FrameworkPropertyMetadata(typeof(FancyFilePicker)));
        }
        public static readonly DependencyProperty HoverBrushProperty = DependencyProperty.Register("HoverBrush", typeof(Brush), typeof(FancyFilePicker),
            new PropertyMetadata(new SolidColorBrush(Colors.AliceBlue)));
        public Brush HoverBrush
        {
            get => (Brush)GetValue(HoverBrushProperty);
            set => SetValue(HoverBrushProperty, value);
        }

        public static readonly DependencyProperty CornersProperty = DependencyProperty.Register("Corners", typeof(CornerRadius), typeof(FancyFilePicker),
            new PropertyMetadata(new CornerRadius(0, 0, 0, 0)));
        public CornerRadius Corners
        {
            get => (CornerRadius)GetValue(CornersProperty); set => SetValue(CornersProperty, value);
        }
        public static readonly DependencyProperty SourcePathProperty = DependencyProperty.Register("Source", typeof(String), typeof(FancyFilePicker),
            new PropertyMetadata(String.Empty, SourcePathChanged));

        public string Source
        {
            get => (String)GetValue(SourcePathProperty);
            set => SetValue(SourcePathProperty, value);
        }
        private static void SourcePathChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            var o = (FancyFilePicker)dp;
            o.onSourcePathChanged(dp, e);
        }
        public void onSourcePathChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var o = (FancyFilePicker)dependencyObject;
            if (pTextBox == null) return;
            pTextBox.Text = (string)e.NewValue;
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            pTextBox = base.Template.FindName("TEXT_FIELD", this) as TextBox;
            pTextBox.PreviewMouseLeftButtonDown += PTextBox_PreviewMouseLeftButtonDown;
        }

        private void PTextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnClick();
        }
    }
}
