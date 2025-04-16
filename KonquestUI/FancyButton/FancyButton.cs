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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace com.KonquestUI.Controls
{
    public class FancyButton : Button
    {
        private BitmapSource PreviousBitmapSource { get; set; }

        #region Dependancy Properties
        public static readonly DependencyProperty HoverBrushProperty;

        public static readonly DependencyProperty SourceProperty; 
        public static readonly DependencyProperty TextProperty;

        public static readonly DependencyProperty CornersProperty;
        public static readonly DependencyProperty ShowSeperatorProperty;
        #endregion

        #region Declarations
        public bool ShowSeperator
        {
            get
            {
                return (bool)GetValue(ShowSeperatorProperty);
            }
            set
            {
                SetValue(ShowSeperatorProperty, value);
            }
        }
        public CornerRadius Corners
        {
            get
            {
                return (CornerRadius)GetValue(CornersProperty);
            }
            set { SetValue(CornersProperty, value); }
        }
        public Brush HoverBrush
        {
            get
            {
                return GetValue(HoverBrushProperty) as Brush;
            }
            set
            {
                SetValue(HoverBrushProperty, value);
            }
        }
        public BitmapSource Source
        {
            get
            {
                return (BitmapSource)GetValue(SourceProperty);
            }
            set
            {
                SetValue(SourceProperty, value);
                PreviousBitmapSource = value;
            }
        }

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }
        #endregion

        #region OnPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Static FancyButton()
        static FancyButton()
        {
           HoverBrushProperty = DependencyProperty.Register("HoverBrush", typeof(Brush), typeof(FancyButton),
                                                             new FrameworkPropertyMetadata(new SolidColorBrush(Colors.SkyBlue)));

            SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(FancyButton), null);
            TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(FancyButton), new PropertyMetadata(""));

            CornersProperty = DependencyProperty.Register("Corners", typeof(CornerRadius), typeof(FancyButton),
                                                 new FrameworkPropertyMetadata(new CornerRadius(0, 0, 0, 0)));

            ShowSeperatorProperty = DependencyProperty.Register("ShowSeperator",
                                                       typeof(bool), typeof(FancyButton), new FrameworkPropertyMetadata(false));

            DefaultStyleKeyProperty.OverrideMetadata(typeof(FancyButton), new FrameworkPropertyMetadata(typeof(FancyButton)));
            IsEnabledProperty.AddOwner(typeof(FancyButton), new FrameworkPropertyMetadata(IsEnabledPropertyChanged));
        }
        #endregion

        private BitmapSource ConvertToGreyscale(BitmapSource source)
        {
            var stride = (source.PixelWidth * source.Format.BitsPerPixel + 7) / 8;
            var pixels = new byte[stride * source.PixelHeight];

            source.CopyPixels(pixels, stride, 0);

            for (int i = 0; i < pixels.Length; i += 4)
            {
                // this works for PixelFormats.Bgra32
                var blue = pixels[i];
                var green = pixels[i + 1];
                var red = pixels[i + 2];
                var gray = (byte)(0.5 * red + 0.5 * green + 0.5 * blue);
                pixels[i] = gray;
                pixels[i + 1] = gray;
                pixels[i + 2] = gray;
            }

            return BitmapSource.Create(
                source.PixelWidth, source.PixelHeight,
                source.DpiX, source.DpiY,
                source.Format, null, pixels, stride);
        }

        private static void IsEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fb = (FancyButton)d;
            if (fb != null)
            {
                fb.OnEnabledPropertyChanged(fb, e);
            }
        }
        public void OnEnabledPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var fb = (FancyButton)sender;
            var v = (bool)e.NewValue;
            var image = fb.Source;
            if (image != null)
            {
                if (v)
                {
                    fb.Source = image;
                }
                else
                {
                    fb.Source = ConvertToGreyscale(image);
                }
            }
        }
    }
}
