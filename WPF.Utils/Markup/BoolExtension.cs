using System;
using System.Windows.Markup;

namespace WPF.Utils.Markup
{
    public class BoolExtension : MarkupExtension
    {
        public BoolExtension() { }

        public BoolExtension(bool value)
        {
            Value = value;
        }

        public bool Value { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Value;
        }
    }
}
