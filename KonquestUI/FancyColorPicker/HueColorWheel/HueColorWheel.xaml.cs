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
using com.KonquestUI;
using System.ComponentModel;

namespace com.KonquestUI.Controls
{
    /// <summary>
    /// Interaction logic for HueColorWheel.xaml
    /// </summary>
    public partial class HueColorWheel : UserControl
    {
        private static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(HueColorWheel), new UIPropertyMetadata(0.0d));
        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public HueColorWheel()
        {
            InitializeComponent();
        }

        private void Path_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ((UIElement)sender).CaptureMouse();
            Path circle = (Path)sender;
            Point mousePos = e.GetPosition(circle);
            UpdateValue(mousePos, circle.ActualWidth, circle.ActualHeight);
        }

        private void Path_MouseMove(object sender, MouseEventArgs e)
        {
            if (!((UIElement)sender).IsMouseCaptured)
                return;

            Path circle = (Path)sender;
            Point mousePos = e.GetPosition(circle);
            UpdateValue(mousePos, circle.ActualWidth, circle.ActualHeight);

        }

        private void Path_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ((UIElement)sender).ReleaseMouseCapture();
        }
        private void UpdateValue(Point mousePos, double width, double height)
        {
            double x = mousePos.X / (width * 2);
            double y = mousePos.Y / (height * 2);

            double length = Math.Sqrt(x * x + y * y);
            if (length == 0)
                return;
            double angle = Math.Acos(x / length);
            if (y < 0)
                angle = -angle;
            angle = angle * 360 / (Math.PI * 2) + 180;
            Value = MathHelper.Clamp(angle, 0, 360);
        }

    }
}
