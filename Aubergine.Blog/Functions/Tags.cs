using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Aubergine.Blog
{
    public static class Tags
    {
        /// <summary>
        ///  the wordpress tags function. 
        /// </summary>
        public static IHtmlString the_tags(this IPublishedContent post, string propertyAlias,
            string before = "Tags: ", string sep = ",", string after = "")
        {

            var tags = post.GetPropertyValue<string[]>(propertyAlias);
                // .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (tags.Any())
            {
                var blogRoot = post.AncestorOrSelf(DocTypes.Blog);
                var tagBase = post.GetPropertyValue<string>(Properties.TagBase, "tags");

                var links = new List<string>();

                foreach (var tag in tags)
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
