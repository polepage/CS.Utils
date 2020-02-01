using Microsoft.Xaml.Behaviors;
using Prism.Common;
using Prism.Regions;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace WPF.Utils.Behaviors
{
    public class RegionContextCommand : Behavior<DependencyObject>
    {
        private ObservableObject<object> _context;

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command",
                                        typeof(ICommand),
                                        typeof(RegionContextCommand),
                                        new PropertyMetadata(CommandChanged));

        public ICommand Command
        {
            get => GetValue(CommandProperty) as ICommand;
            set => SetValue(CommandProperty, value);
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

        private static void CommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as RegionContextCommand)?.Notify();
        }

        private void ContextChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify();
        }

        private void Notify()
        {
            Command?.Execute(_context?.Value);
        }
    }
}
