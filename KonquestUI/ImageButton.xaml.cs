using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace com.Konquest.UI
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ImageButton : UserControl
    {
        #region Declariations 
        private Color _defaultGreyedOutColor = Colors.DarkGray;
        #endregion

        #region Dependancy Properties
        public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register("Icon", typeof(ImageSource), typeof(ImageButton), null);
        public static readonly DependencyProperty HoverBrushProperty =
            DependencyProperty.Register("HoverBrush", typeof(Brush), typeof(ImageButton), null);
        public static readonly DependencyProperty GreyedOutBrushProperty = DependencyProperty.Register("GreyedOutBrush", typeof(Brush), typeof(ImageButton), null);
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(ImageButton), null);

        private double _imageHeight, _imageWidth;

        private static void IconPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d != null && e.NewValue != null)
            {
                (d as Image).Source = (ImageSource)e.NewValue;
            }
        }
        public double ImageHeight
        {
            get => _imageHeight;
            set
            {
                _imageHeight = value;
                OnPropertyChanged(nameof(ImageHeight));
            }
        }
        public double ImageWidth
        {
            get => _imageWidth;
            set
            {
                _imageWidth = value;
                OnPropertyChanged(nameof(ImageWidth));
            }
        }
        [Bindable(true)]
        [Localizability(LocalizationCategory.NeverLocalize)]
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);   
            set => SetValue(CommandProperty, value);
        }
        public Brush GreyedOutBrush
        {
            get
            {
                if (GreyedOutBrush == null)
                {
                    SetValue(GreyedOutBrushProperty, new SolidColorBrush(_defaultGreyedOutColor));

                    return (Brush)GetValue(GreyedOutBrushProperty);
                }
                return (Brush)GetValue(GreyedOutBrushProperty);
            }
            set
            {
                SetValue(GreyedOutBrushProperty, value);
            }
        }
        public Brush HoverBrush
        {
            get => (Brush)GetValue(HoverBrushProperty);
            set => SetValue(HoverBrushProperty, value);
        }
        public ImageSource Icon
        {
            get => (ImageSource)GetValue(IconProperty);
            set
            {
                SetValue(IconProperty, value);
            }
        }

        public Brush OldBackgroundBrush { get; set; }
        #endregion
        #region Routed Events

        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent(
       name: "Click",
       routingStrategy: RoutingStrategy.Bubble,
       handlerType: typeof(RoutedEventHandler),
       ownerType: typeof(ImageButton));

        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        void RaiseClickRoutedEvent()
        {
            // Create a RoutedEventArgs instance.
            if (Command != null)
            {
                Command.Execute(null);
            }
            else
            {
                RoutedEventArgs routedEventArgs = new RoutedEventArgs(ClickEvent);
                // Raise the event, which will bubble up through the element tree.
                RaiseEvent(routedEventArgs);
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

        public ImageButton()
        {
            InitializeComponent();
            this.DataContext = this;
            ImageHeight = this.ActualHeight - 2;
            ImageWidth = this.ActualWidth - 2;
        }

        private void MAIN_CONTENT_Loaded(object sender, RoutedEventArgs e)
        {

        }

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
                var gray = (byte)(0.2126 * red + 0.7152 * green + 0.0722 * blue);
                pixels[i] = _defaultGreyedOutColor.R; // gray;
                pixels[i + 1] = _defaultGreyedOutColor.R; // gray;
                pixels[i + 2] = _defaultGreyedOutColor.R; // grey;
            }

            return BitmapSource.Create(
                source.PixelWidth, source.PixelHeight,
                source.DpiX, source.DpiY,
                source.Format, null, pixels, stride);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.BorderBrush = this.Background;
            OldBackgroundBrush = this.Background;

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Background = HoverBrush;
            var BorderHoverBrushColor = ((SolidColorBrush)HoverBrush).Color;
            var newDarkerColor = new Color();
            newDarkerColor.A = 0;
            newDarkerColor.B = 128;
            newDarkerColor.R = 128;
            newDarkerColor.G = 128;
            Brush darkerBorderHoverBrushColor = new SolidColorBrush(Color.Subtract(BorderHoverBrushColor, newDarkerColor));

            this.BorderBrush = darkerBorderHoverBrushColor;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Background = OldBackgroundBrush;
            this.BorderBrush = this.Background;
        }

        private void UserControl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            RaiseClickRoutedEvent();
        }

        private void UserControl_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            if ((bool)e.NewValue == false)
            {
                if (Icon == null)
                    return;

                var image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(Icon.ToString());
                image.EndInit();

                BUTTON_ICON.Source = ConvertToGreyscale(image);
            }
            else
            {
                if (Icon == null) return;

                var image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri($"{Icon.ToString()}");
                image.EndInit();
                BUTTON_ICON.Source = image;
            }
        }
    }
}
