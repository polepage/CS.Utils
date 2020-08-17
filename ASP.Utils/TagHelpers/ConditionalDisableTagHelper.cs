using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ASP.Utils.TagHelpers
{
    [HtmlTargetElement(Attributes = "conditional-disable")]
    public class ConditionalDisableTagHelper: TagHelper
    {
        public bool ConditionalDisable { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ConditionalDisable)
            {
                output.Attributes.SetAttribute("disabled", "");
            }
        }
    }
}
