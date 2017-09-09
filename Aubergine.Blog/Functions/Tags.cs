using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Aubergine.Blog
{
    /// <summary>
    ///  helper class (mimics wordpress the_tags function)
    /// </summary>
    public static class Tags
    {
        public static IHtmlString the_tags(this IPublishedContent post, 
            string propertyAlias, 
            string before = "Tags: ", string sep = ",", string after = "")
        {
            var tags = post.GetPropertyValue<string[]>(propertyAlias);

            if (tags.Any())
            {
                var blogRoot = post.AncestorOrSelf(Blog.Presets.DocTypes.Blog);
                var tagBase = blogRoot.GetPropertyValue<string>(Blog.Presets.Properties.TagBase, "tags");

                var links = new List<string>();

                foreach(var tag in tags)
                {
                    var url = $"{blogRoot.Url}{tagBase}/{tag}";
                    links.Add($"<a href=\"{url}\">{tag}</a>");
                }

                if (links.Any())
                {
                    return new HtmlString($"{before}{string.Join(sep, links)}{after}");
                }
            }

            return new HtmlString("");

        }
    }
}
