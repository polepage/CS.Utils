using Microsoft.Xaml.Behaviors;
using Prism.Common;
using Prism.Regions;
using System.ComponentModel;
using System.Windows;

namespace WPF.Utils.Behaviors
{
    public class BindRegionContext: Behavior<DependencyObject>
    {
        private ObservableObject<object> _context;

        public static readonly DependencyProperty ContextProperty =
            DependencyProperty.Register("Context",
                                        typeof(object),
                                        typeof(BindRegionContext));

        public object Context
        {
            get => GetValue(ContextProperty);
            set => SetValue(ContextProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            _context = RegionContext.GetObservableContext(AssociatedObject);
            _context.PropertyChanged += ContextChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (_context != null)
            {
                _context.PropertyChanged -= ContextChanged;
            }
        }

        private void ContextChanged(object sender, PropertyChangedEventArgs e)
        {
            Context = _context.Value;
        }
    }
}
