﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace com.KonquestUI.Controls
{

    public class FancyImageButton : ButtonBase
    {
        protected bool IsGrayscaled => ImageSource is FormatConvertedBitmap;
        static FancyImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FancyImageButton), new FrameworkPropertyMetadata(typeof(FancyImageButton)));
            IsEnabledProperty.OverrideMetadata(typeof(FancyImageButton), new FrameworkPropertyMetadata(true, IsEnabledPropertyChanged));
        }

        private static void IsEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FancyImageButton s && s.IsEnabled == s.IsGrayscaled)
            {
                s.UpdateImageSource();
            }
        }
        protected void UpdateImageSource()
        {
            if (ImageSource == null)
                return;

            if (IsEnabled)
            {
                if (IsGrayscaled)
                {
                    ImageSource = ((FormatConvertedBitmap)ImageSource).Source;
                    OpacityMask = null;
                }
            }
            else
            {
                if (!IsGrayscaled)
                {
                    if (ImageSource is BitmapSource bitmapImage)
                    {
                        ImageSource = new FormatConvertedBitmap(bitmapImage, PixelFormats.Gray8, null, 0);
                        OpacityMask = new ImageBrush(bitmapImage);
                    }
                }
            }
        }

        public static readonly DependencyProperty HighlightBrushProperty = DependencyProperty.Register("HighlightBrush", typeof(Brush), typeof(FancyImageButton),
            new FrameworkPropertyMetadata(new SolidColorBrush(Colors.SkyBlue)));
        public static readonly DependencyProperty HighlightStrokeBrushProperty = DependencyProperty.Register("HighlightStrokeBrush", typeof(Brush), typeof(FancyImageButton),
            new FrameworkPropertyMetadata(new SolidColorBrush(Colors.SkyBlue)));
        public static readonly DependencyProperty HighlightStrokeThicknessProperty = DependencyProperty.Register("HighlightStrokeThickness", typeof(Thickness), typeof(FancyImageButton),
            new FrameworkPropertyMetadata(new Thickness(1)));
        public static readonly DependencyProperty HighlightOpacityProperty = DependencyProperty.Register("HighlightOpacity", typeof(Double), typeof(FancyImageButton),
            new FrameworkPropertyMetadata(1.0));
        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(FancyImageButton), null);
        public static readonly DependencyProperty ImageSourcesProperty = DependencyProperty.Register("ImageSources", typeof(ObservableCollection<BitmapImage>), typeof(FancyImageButton), null);
        public static readonly DependencyProperty CornersProperty = DependencyProperty.Register("Corners", typeof(CornerRadius), typeof(FancyImageButton),
            new FrameworkPropertyMetadata(new CornerRadius(0, 0, 0, 0)));

        public Brush HighlightBrush
        {
            get => (Brush)GetValue(HighlightBrushProperty);
            set => SetValue(HighlightBrushProperty, value);
        }
        public Brush HighlightStrokeBrush
        {
            get => (Brush)GetValue(HighlightStrokeBrushProperty);
            set => SetValue(HighlightStrokeBrushProperty, value);
        }
        public Thickness HighlightStrokeThickness
        {
            get => (Thickness)GetValue(HighlightStrokeThicknessProperty);
            set => SetValue(HighlightStrokeThicknessProperty, value);
        }
        public Double HighlightOpacity
        {
            get => (double)GetValue(HighlightOpacityProperty);
            set => SetValue(HighlightOpacityProperty, value);
        }
        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        private ObservableCollection<BitmapImage> _imageSources = new ObservableCollection<BitmapImage>();
        public ObservableCollection<BitmapImage> ImageSources
        {
            get
            {
                return _imageSources;
            }
            set
            {
                _imageSources = value;
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
    }
}
