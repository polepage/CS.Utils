using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace WPF.Utils.Controls
{
    public class WatermarkAdorner : Adorner
    {
        private readonly ContentPresenter _contentPresenter;

        public WatermarkAdorner(UIElement adornedElement, object watermark)
            : base(adornedElement)
        {
            IsHitTestVisible = false;

            _contentPresenter = new ContentPresenter
            {
                Content = watermark,
                Opacity = 0.5,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(3, 0, 0, 0)
            };

            SetBinding(VisibilityProperty, new Binding("Visibility")
            {
                Source = adornedElement,
                Mode = BindingMode.OneWay
            });
        }

        protected override int VisualChildrenCount => 1;

        private Control Control => (Control)AdornedElement;

        protected override Visual GetVisualChild(int index)
        {
            return _contentPresenter;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            _contentPresenter.Measure(Control.RenderSize);
            return Control.RenderSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _contentPresenter.Arrange(new Rect(finalSize));
            return finalSize;
        }
    }
}
