using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WPF.Utils.Templates
{
    class TemplatedMultiSelectorCheckboxListener: Behavior<CheckBox>
    {
        public static readonly DependencyProperty OpenProperty =
            DependencyProperty.Register("Open",
                                        typeof(bool),
                                        typeof(TemplatedMultiSelectorCheckboxListener),
                                        new PropertyMetadata(PropertyChanged));

        public static readonly DependencyProperty SelectorProperty =
            DependencyProperty.Register("Selector",
                                        typeof(MultiSelector),
                                        typeof(TemplatedMultiSelectorCheckboxListener),
                                        new PropertyMetadata(PropertyChanged));

        public bool Open
        {
            get => (bool)GetValue(OpenProperty);
            set => SetValue(OpenProperty, value);
        }

        public MultiSelector Selector
        {
            get => GetValue(SelectorProperty) as MultiSelector;
            set => SetValue(SelectorProperty, value);
        }

        private static void PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TemplatedMultiSelectorCheckboxListener behavior)
            {
                behavior.UpdateCheckbox();
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            UpdateCheckbox();
        }

        private void UpdateCheckbox()
        {
            if (Open && Selector != null && Selector.SelectedItems != null)
            {
                bool newValue = Selector.SelectedItems.Contains(AssociatedObject.Content);
                bool? oldValue = AssociatedObject.IsChecked;
                if (newValue != oldValue)
                {
                    AssociatedObject.IsChecked = newValue;
                }
            }
        }
    }
}
