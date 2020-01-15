using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WPF.Utils.Controls
{
    public class TemplatedMultiSelector: MultiSelector
    {
        #region Templates
        public static readonly DependencyProperty SelectedItemTemplateProperty =
            DependencyProperty.Register("SelectedItemTemplate",
                                        typeof(DataTemplate),
                                        typeof(TemplatedMultiSelector));

        public DataTemplate SelectedItemTemplate
        {
            get => (DataTemplate)GetValue(SelectedItemTemplateProperty);
            set => SetValue(SelectedItemTemplateProperty, value);
        }

        public static readonly DependencyProperty SelectedItemTemplateSelectorProperty =
            DependencyProperty.Register("SelectedItemTemplateSelector",
                                        typeof(DataTemplateSelector),
                                        typeof(TemplatedMultiSelector));

        public DataTemplateSelector SelectedItemTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(SelectedItemTemplateSelectorProperty);
            set => SetValue(SelectedItemTemplateSelectorProperty, value);
        }

        public static readonly DependencyProperty SelectedItemStringFormatProperty =
            DependencyProperty.Register("SelectedItemStringFormat",
                                        typeof(string),
                                        typeof(TemplatedMultiSelector));

        public string SelectedItemStringFormat
        {
            get => (string)GetValue(SelectedItemStringFormatProperty);
            set => SetValue(SelectedItemStringFormatProperty, value);
        }
        #endregion
    }
}
