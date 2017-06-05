using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Logging;
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
            if (content != null && content.DocumentTypeAlias == DocTypes.BlogPost)
            {
                var format = content.GetPropertyValue<string>(Properties.Permalinks, true,
                     "%year%/%month%/%day%/%postname%/").Trim(new char[] { '-' });

                var date = content.GetPropertyValue<DateTime>(Properties.PublishDate, content.CreateDate);
                if (date != null)
                {
                    var baseUrl = content.Parent.Url;
                    if (content.Parent.DocumentTypeAlias == DocTypes.BlogPosts)
                        baseUrl = content.Parent.Parent.Url;

                    if (!baseUrl.EndsWith("/"))
                        baseUrl += "/";

                    var subs = new Dictionary<string, string>();
                    subs.Add("%postname%", content.UrlName);
                    subs.Add("%post_id%", content.Id.ToString());

                    return baseUrl + FormatBlogUrl(format, date, subs);
                }
            }
            return null;
        }

        private string FormatBlogUrl(string format, DateTime publishDate, Dictionary<string, string> subs)
        {
            if (subs == null)
                subs = new Dictionary<string, string>();

            subs.Add("%year%", publishDate.ToString("yyyy"));
            subs.Add("%monthnum%", publishDate.ToString("MM"));
            subs.Add("%monthname%", publishDate.ToString("MMMM"));
            subs.Add("%day%", publishDate.ToString("dd"));
            subs.Add("%hour%", publishDate.ToString("hh"));
            subs.Add("%minute%", publishDate.ToString("mm"));
            subs.Add("%second%", publishDate.ToString("ss"));

            return format.ReplaceMany(subs);
        }
    }
}
