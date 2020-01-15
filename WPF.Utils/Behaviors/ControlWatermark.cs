using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using WPF.Utils.Controls;

namespace WPF.Utils.Behaviors
{
    public static class ControlWatermark
    {
        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.RegisterAttached("Watermark",
                                                typeof(object),
                                                typeof(ControlWatermark),
                                                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnWatermarkChanged)));

        public static object GetWatermark(DependencyObject d)
        {
            return d.GetValue(WatermarkProperty);
        }

        public static void SetWatermark(DependencyObject d, object value)
        {
            d.SetValue(WatermarkProperty, value);
        }

        private static void OnWatermarkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Control control)
            {
                control.Loaded += ShowOrNotWatermark;
                control.GotKeyboardFocus += ShowOrHideWatermark;
                control.LostKeyboardFocus += ShowOrNotWatermark;
            }

            if (d is TextBox textBox)
            {
                textBox.TextChanged += ShowOrHideWatermark;
            }

            if (d is Selector selector)
            {
                selector.SelectionChanged += ShowOrHideWatermark;
            }
        }

        private static void ShowOrNotWatermark(object sender, RoutedEventArgs e)
        {
            if (sender is Control control && ShouldShowWatermark(control))
            {
                ShowWatermark(control);
            }
        }

        private static void ShowOrHideWatermark(object sender, RoutedEventArgs e)
        {
            if (sender is Control control)
            {
                if (ShouldShowWatermark(control))
                {
                    ShowWatermark(control);
                }
                else
                {
                    RemoveWatermark(control);
                }
            }
        }

        private static bool ShouldShowWatermark(Control control)
        {
            if (control is Selector selector)
            {
                return selector.SelectedItem == null;
            }

            if (control is TextBox textBox)
            {
                return string.IsNullOrEmpty(textBox.Text);
            }

            return false;
        }

        private static void ShowWatermark(Control control)
        {
            var layer = AdornerLayer.GetAdornerLayer(control);
            if (layer != null)
            {
                Adorner[] adorners = layer.GetAdorners(control);
                if (adorners != null && adorners.Any(a => a is WatermarkAdorner))
                {
                    foreach (WatermarkAdorner adorner in adorners)
                    {
                        adorner.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    layer.Add(new WatermarkAdorner(control, GetWatermark(control)));
                }
            }
        }

        private static void RemoveWatermark(Control contro)
        {
            var layer = AdornerLayer.GetAdornerLayer(contro);
            if (layer != null)
            {
                Adorner[] adorners = layer.GetAdorners(contro);
                if (adorners == null)
                {
                    return;
                }

                foreach (WatermarkAdorner adorner in adorners)
                {
                    adorner.Visibility = Visibility.Hidden;
                    layer.Remove(adorner);
                }
            }
        }
    }
}
