using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WPF.Utils.Behaviors
{
    public class FeedItemsControlFromMultiSelector: Behavior<ItemsControl>
    {
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source",
                                        typeof(MultiSelector),
                                        typeof(FeedItemsControlFromMultiSelector),
                                        new PropertyMetadata(SourceChanged));

        public MultiSelector Source
        {
            get => GetValue(SourceProperty) as MultiSelector;
            set => SetValue(SourceProperty, value);
        }

        private static void SourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FeedItemsControlFromMultiSelector behavior)
            {
                behavior.UpdateSource();
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            UpdateSource();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.ItemsSource = null;
        }

        private void UpdateSource()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.ItemsSource = Source?.SelectedItems;
            }
        }
    }
}
