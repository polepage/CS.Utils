using Microsoft.Xaml.Behaviors;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace WPF.Utils.Actions
{
    public class ListBoxSelectionChangedTrigger : TriggerAction<ListBox>
    {
        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register("Target",
                                        typeof(IList),
                                        typeof(ListBoxSelectionChangedTrigger),
                                        new PropertyMetadata(TargetChanged));

        public IList Target
        {
            get => GetValue(TargetProperty) as IList;
            set => SetValue(TargetProperty, value);
        }

        private static void TargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ListBoxSelectionChangedTrigger trigger)
            {
                trigger.Target?.Clear();
                trigger.Update(trigger.Target, trigger.AssociatedObject?.SelectedItems, null);
            }
        }

        protected override void Invoke(object parameter)
        {
            if (parameter is SelectionChangedEventArgs args)
            {
                Update(Target, args.AddedItems, args.RemovedItems);
            }
        }

        private void Update(IList target, IList added, IList removed)
        {
            if (target == null)
            {
                return;
            }

            if (removed != null)
            {
                foreach (object item in removed)
                {
                    target.Remove(item);
                }
            }

            if (added != null)
            {
                foreach (object item in added)
                {
                    target.Add(item);
                }
            }
        }
    }
}
