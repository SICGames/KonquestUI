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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using com.KonquestUI.Base.Editors;
using com.KonquestUI.Controls;

namespace com.KonquestUI.PropertyGrid.Editors
{
    /// <summary>
    /// Interaction logic for LabelControl.xaml
    /// </summary>
    public partial class LabelControl : EditorControlsBase
    {
        private static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(String), typeof(LabelControl), new PropertyMetadata(""));
        private static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register(nameof(PropertyName), typeof(String), typeof(LabelControl), new PropertyMetadata(""));
        private static readonly DependencyProperty PropertyDescriptionProperty = DependencyProperty.Register(nameof(PropertyDescription), typeof(String), typeof(LabelControl), new PropertyMetadata(""));

        private static readonly DependencyProperty NudgeHighlightColorProperty = DependencyProperty.Register(nameof(NudgeHighliteColor), typeof(Color), typeof(LabelControl), new PropertyMetadata(Color.FromArgb(155, 255, 255, 255)));
        private static readonly DependencyProperty NudgeBackgroundColorProperty = DependencyProperty.Register(nameof(NudgeBackgroundColor), typeof(Color), typeof(LabelControl), new PropertyMetadata(Color.FromArgb(255, 51, 51, 51)));
        private static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(nameof(IsExpanded), typeof(bool), typeof(LabelControl),
            new PropertyMetadata(false, new PropertyChangedCallback(onIsExpandedPropertyChanged)));

        private static readonly DependencyProperty IsExpandableProperty = DependencyProperty.Register(nameof(IsExpandable), typeof(bool), typeof(LabelControl),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(onIsExpandablePropertyChanged)));
        private Storyboard sb;

        public bool IsExpandable
        {
            get => (bool)GetValue(IsExpandableProperty);
            set => SetValue(IsExpandableProperty, value);
        }

        public bool IsExpanded
        {
            get => (bool)GetValue(IsExpandedProperty);
            set
            {
                SetValue(IsExpandedProperty, value);
            }
        }
        public Color NudgeHighliteColor
        {
            get => (Color)GetValue(NudgeHighlightColorProperty);
            set => SetValue(NudgeHighlightColorProperty, value);
        }
        public Color NudgeBackgroundColor
        {
            get => (Color)GetValue(NudgeBackgroundColorProperty);
            set => SetValue(NudgeBackgroundColorProperty, value);
        }

        public String Text
        {
            get => (String)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        public String PropertyName
        {
            get => (String)GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);

        }
        public String PropertyDescription
        {
            get => (String)GetValue(PropertyDescriptionProperty);
            set => SetValue(PropertyDescriptionProperty, value);
        }

        public LabelControl() : base()
        {
            InitializeComponent();
            sb = new Storyboard();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private static void onIsExpandedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabelControl label = (LabelControl)d;
            if (label.IsExpanded)
            {
                //System.Diagnostics.Debug.WriteLine("Expanded.");
                //RotateTransform rt = new RotateTransform();
                //rt.Angle = 90;
                //label.ExpandArrow.RenderTransform = rt;
                label.sb = label.FindResource("ExpandedAnim") as Storyboard;
                label.sb.Begin();
            }
            else
            {
                //System.Diagnostics.Debug.WriteLine("Not Expanded.");
                //RotateTransform rt = new RotateTransform();
                //rt.Angle = 0;
                //label.ExpandArrow.RenderTransform = rt;
                label.sb = label.FindResource("NotExpandedAnim") as Storyboard;
                label.sb.Begin();
            }
        }
        private static void onIsExpandablePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabelControl label = (LabelControl)d;
            if (label.IsExpandable)
            {
                label.ExpandArrow.Visibility = Visibility.Visible;
            }
            else
            {
                label.ExpandArrow.Visibility = Visibility.Hidden;
            }
        }
        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            HoverEventArgs args = new HoverEventArgs(HoverEvent, PropertyName, PropertyDescription);
            RaiseEvent(args);
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {

        }
    }
}
