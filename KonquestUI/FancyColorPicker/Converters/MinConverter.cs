using System;
using System.Linq;
using System.Windows;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace com.KonquestUI.Converters
{
    class MinConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Min();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
