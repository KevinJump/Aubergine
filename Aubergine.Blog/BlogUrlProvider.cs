using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Routing;

namespace Aubergine.Blog
{
    public class BlogUrlProvider : IUrlProvider
    {
        public IEnumerable<string> GetOtherUrls(UmbracoContext umbracoContext, int id, Uri current)
        {
            return Enumerable.Empty<string>();
        }

        public string GetUrl(UmbracoContext umbracoContext, int id, Uri current, UrlProviderMode mode)
        {
            var content = umbracoContext.ContentCache.GetById(id);
            if (content != null && content.DocumentTypeAlias == Blog.Presets.DocTypes.BlogPost)
            {
                var format = content.GetPropertyValue<string>
                    (Blog.Presets.Properties.Permalinks, true, "%year%/%monthnum%/%day%/%postname%/")
                    .Trim(new char[] { '-' });

                var date = content.GetPropertyValue<DateTime>(Blog.Presets.Properties.PublishDate, content.CreateDate);
                if (date != null && date > DateTime.MinValue)
                {
                    var baseUrl = "";
                    var blogRoot = content.AncestorOrSelf(Blog.Presets.DocTypes.Blog);
                    if (blogRoot != null)
                    {
                        baseUrl = blogRoot.Url;
                    }
                    else
                    {
                        baseUrl = content.Parent.Url;
                    }

                    if (!baseUrl.EndsWith("/"))
                        baseUrl += "/";

                    var replacements = new Dictionary<string, string>();
                    replacements.Add("%postname%", content.UrlName);
                    replacements.Add("%post_id", content.Id.ToString());

                    return baseUrl + FormatBlogUrl(format, date, replacements);
                }
            }
            return null;
        }

        private string FormatBlogUrl(string format, DateTime publishDate, Dictionary<string, string> replacements)
        {
            if (replacements == null)
                replacements = new Dictionary<string, string>();


            replacements.Add("%year%", publishDate.ToString("yyyy"));
            replacements.Add("%monthnum%", publishDate.ToString("MM"));
            replacements.Add("%monthname%", publishDate.ToString("MMMM"));
            replacements.Add("%day%", publishDate.ToString("dd"));
            replacements.Add("%hour%", publishDate.ToString("hh"));
            replacements.Add("%minute%", publishDate.ToString("mm"));
            replacements.Add("%second%", publishDate.ToString("ss"));

            return format.ReplaceMany(replacements);
        }

    }
}
