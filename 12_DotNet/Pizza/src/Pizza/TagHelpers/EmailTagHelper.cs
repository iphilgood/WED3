using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Pizza.TagHelpers
{
    public class EmailTagHelper : TagHelper
    {
        public string MailFor { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a"; // Replaces <email> with <a> tag
            output.Attributes.SetAttribute("href", "mailto:" + MailFor);
            output.Content.SetContent(MailFor);
        }
    }





    /*
            public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
            {
                output.TagName = "a";
                var content = await output.GetChildContentAsync();
                var target = content.GetContent();
                output.Attributes.SetAttribute("href", "mailto:" + target);
                output.Content.SetContent(target);
            }*/
}
