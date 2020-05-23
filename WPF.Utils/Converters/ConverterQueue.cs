using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace WPF.Utils.Converters
{
    [ContentProperty("Converters")]
    public class ConverterQueue : IValueConverter
    {
        public List<IValueConverter> Converters { get; set; } = new List<IValueConverter>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object result = value;
            foreach (IValueConverter converter in Converters)
            {
                result = converter.Convert(result, targetType, parameter, culture);
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object result = value;
            foreach (IValueConverter converter in Converters.Reverse<IValueConverter>())
            {
                result = converter.ConvertBack(result, targetType, parameter, culture);
            }

            return result;
        }
    }
}
