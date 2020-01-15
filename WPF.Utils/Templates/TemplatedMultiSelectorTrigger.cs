using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace WPF.Utils.Templates
{
    class TemplatedMultiSelectorTrigger : TriggerAction<ToggleButton>
    {
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data",
                                        typeof(object),
                                        typeof(TemplatedMultiSelectorTrigger));

        public static readonly DependencyProperty SelectorProperty =
            DependencyProperty.Register("Selector",
                                        typeof(MultiSelector),
                                        typeof(TemplatedMultiSelectorTrigger));

        public object Data
        {
            get => GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        public MultiSelector Selector
        {
            get => GetValue(SelectorProperty) as MultiSelector;
            set => SetValue(SelectorProperty, value);
        }

        protected override void Invoke(object parameter)
        {
            if (Selector != null && Data != null && parameter is RoutedEventArgs e)
            {
                if (e.RoutedEvent.Name == ToggleButton.CheckedEvent.Name)
                {
                    Selector.SelectedItems.Add(Data);
                }
                else if (e.RoutedEvent.Name == ToggleButton.UncheckedEvent.Name)
                {
                    Selector.SelectedItems.Remove(Data);
                }

                e.Handled = true;
            }
        }
    }
}
