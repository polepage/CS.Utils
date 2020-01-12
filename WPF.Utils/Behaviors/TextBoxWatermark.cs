using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using WPF.Utils.Controls;

namespace WPF.Utils.Behaviors
{
    public static class TextBoxWatermark
    {
        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.RegisterAttached("Watermark",
                                                typeof(string),
                                                typeof(TextBoxWatermark),
                                                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnWatermarkChanged)));

        public static string GetWatermark(DependencyObject d)
        {
            return (string)d.GetValue(WatermarkProperty);
        }

        public static void SetWatermark(DependencyObject d, string value)
        {
            d.SetValue(WatermarkProperty, value);
        }

        private static void OnWatermarkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                textBox.Loaded += ShowOrNotWatermark;
                textBox.GotKeyboardFocus += ShowOrHideWatermark;
                textBox.LostKeyboardFocus += ShowOrNotWatermark;
                textBox.TextChanged += ShowOrHideWatermark;
            }
        }

        private static void ShowOrNotWatermark(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && ShouldShowWatermark(textBox))
            {
                ShowWatermark(textBox);
            }
        }

        private static void ShowOrHideWatermark(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (ShouldShowWatermark(textBox))
                {
                    ShowWatermark(textBox);
                }
                else
                {
                    RemoveWatermark(textBox);
                }
            }
        }

        private static bool ShouldShowWatermark(TextBox textBox)
        {
            return string.IsNullOrEmpty(textBox.Text);
        }

        private static void ShowWatermark(TextBox textBox)
        {
            var layer = AdornerLayer.GetAdornerLayer(textBox);
            if (layer != null)
            {
                Adorner[] adorners = layer.GetAdorners(textBox);
                if (adorners != null && adorners.Any(a => a is WatermarkAdorner))
                {
                    foreach (WatermarkAdorner adorner in adorners)
                    {
                        adorner.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    layer.Add(new WatermarkAdorner(textBox, GetWatermark(textBox)));
                }
            }
        }

        private static void RemoveWatermark(TextBox textBox)
        {
            var layer = AdornerLayer.GetAdornerLayer(textBox);
            if (layer != null)
            {
                Adorner[] adorners = layer.GetAdorners(textBox);
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
