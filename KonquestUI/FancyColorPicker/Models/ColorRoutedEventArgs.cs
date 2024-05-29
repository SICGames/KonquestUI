using System.Windows;
using System.Windows.Media;

namespace com.KonquestUI
{
    public class ColorRoutedEventArgs : RoutedEventArgs
    {
        public ColorRoutedEventArgs(RoutedEvent routedEvent, Color color) : base(routedEvent)
        {
            Color = color;
        }

        public Color Color { get; private set; }
    }
}
