using CS.Utils.Actions;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WPF.Utils.Behaviors
{
    public class BindableSelectedItems: Behavior<MultiSelector>
    {
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems",
                                        typeof(IList),
                                        typeof(BindableSelectedItems),
                                        new PropertyMetadata(SelectedItemsChanged));

        public IList SelectedItems
        {
            get => GetValue(SelectedItemsProperty) as IList;
            set => SetValue(SelectedItemsProperty, value);
        }

        private static void SelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BindableSelectedItems behavior)
            {
                behavior.UnregisterBoundCollectionEvents(e.OldValue as INotifyCollectionChanged);
                behavior.RegisterBoundCollectionEvents(e.NewValue as INotifyCollectionChanged);

                using (behavior.IgnoreSourceEvents())
                {
                    behavior.ResetSource(behavior.AssociatedObject?.SelectedItems, behavior.SelectedItems);
                }
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            
            using (IgnoreSourceEvents())
            {
                ResetSource(AssociatedObject?.SelectedItems, SelectedItems);
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            UnregisterSourceEvents();
        }

        private void RegisterSourceEvents()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.SelectionChanged += SourceSelectionChanged;
            }
        }

        private void UnregisterSourceEvents()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.SelectionChanged -= SourceSelectionChanged;
            }
        }

        private void RegisterBoundCollectionEvents(INotifyCollectionChanged collection)
        {
            if (collection != null)
            {
                collection.CollectionChanged += BoundCollectionChanged;
            }
        }

        private void UnregisterBoundCollectionEvents(INotifyCollectionChanged collection)
        {
            if (collection != null)
            {
                collection.CollectionChanged -= BoundCollectionChanged;
            }
        }

        private IDisposable IgnoreSourceEvents()
        {
            UnregisterSourceEvents();
            return new DisposableAction(RegisterSourceEvents);
        }

        private IDisposable IgnoreBoundCollectionEvents()
        {
            UnregisterBoundCollectionEvents(SelectedItems as INotifyCollectionChanged);
            return new DisposableAction(() => RegisterBoundCollectionEvents(SelectedItems as INotifyCollectionChanged));
        }

        private void SourceSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using(IgnoreBoundCollectionEvents())
            {
                PushCollectionModification(SelectedItems, e.AddedItems, e.RemovedItems);
            }
        }

        private void BoundCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            using (IgnoreSourceEvents())
            {
                if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    AssociatedObject?.SelectedItems?.Clear();
                }
                else
                {
                    PushCollectionModification(AssociatedObject?.SelectedItems, e.NewItems, e.OldItems);
                }
            }
        }

        private void ResetSource(IList source, IList replacement)
        {
            source?.Clear();
            PushCollectionModification(source, replacement, null);
        }

        private void PushCollectionModification(IList target, IList newItems, IList oldItems)
        {
            if (target == null)
            {
                return;
            }

            if (oldItems != null)
            {
                foreach (object item in oldItems)
                {
                    target.Remove(item);
                }
            }

            if (newItems != null)
            {
                foreach (object item in newItems)
                {
                    target.Add(item);
                }
            }
        }
    }
}
