using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPF.Utils.Templates
{
    public class ComboBoxTemplateSelector: DataTemplateSelector
    {
        public DataTemplate DropDownItemTemplate { get; set; }
        public DataTemplate SelectedItemTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var visualItem = container;
            while (visualItem != null)
            {
                if (visualItem is ComboBoxItem)
                {
                    return DropDownItemTemplate;
                }
                
                if (visualItem is ComboBox)
                {
                    return SelectedItemTemplate;
                }

                visualItem = VisualTreeHelper.GetParent(visualItem);
            }

            return base.SelectTemplate(item, container);
        }
    }
}
