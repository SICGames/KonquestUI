using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class FancyTagBox : Control
    {
        WrapPanel TagsViewParent { get; set; }
        TextBox TextBox { get; set; }

        static FancyTagBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FancyTagBox), new FrameworkPropertyMetadata(typeof(FancyTagBox)));
        }
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            TextBox = base.Template.FindName("TAGS_TEXTBOX", this) as TextBox;
            TagsViewParent = base.Template.FindName("TAGS_BOX", this) as WrapPanel;
            TextBox.PreviewKeyDown += TextBox_PreviewKeyDown;
            UpdateTagBoxView();
        }

        private void AddTagItemElement(string tag)
        {
            FancyTagItem ti = new FancyTagItem();
            ti.Label = tag;
            TagsViewParent.Children.Add(ti);
        }
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var t = (TextBox)sender;
            if (string.IsNullOrEmpty(t.Text))
                return;

            if (e.Key == Key.Space)
            {
                Tags.Add(t.Text);

                UpdateTagBoxView();
                TextBox.Text = "";
            }
        }

        public static readonly DependencyProperty TagsProperty = DependencyProperty.Register("Tags", typeof(ObservableCollection<String>), typeof(FancyTagBox),
            new PropertyMetadata(new ObservableCollection<String>(), TagsChanged));

        public ObservableCollection<String> Tags
        {
            get => (ObservableCollection<String>)GetValue(TagsProperty);
            set => SetValue(TagsProperty, value);
        }

        private static void TagsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ftb = (FancyTagBox)d;
            if (ftb != null)
            {
                ftb.onTagsChanged(ftb, e);
            }
        }
        public void onTagsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateTagBoxView();
        }
        public void RemoveSelectedTagItem(String item)
        {
            foreach (var tag in Tags)
            {
                if (tag.Equals(item))
                {
                    Tags.Remove(tag);
                    break;
                }
            }
            UpdateTagBoxView();

        }
        private void UpdateTagBoxView()
        {
            if (TextBox != null && TagsViewParent != null)
            {
                TagsViewParent.Children.Clear();
                if (Tags == null)
                    Tags = new ObservableCollection<String>();

                foreach (string item in Tags)
                {
                    AddTagItemElement(item);
                }
            }
        }
    }
}
