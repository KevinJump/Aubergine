using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Web.Routing;
using Umbraco.Core.Cache;
using System.Web.Caching;
using Umbraco.Web;
using Umbraco.Core.Models;

namespace Aubergine.Blog
{
    public class BlogContentFinder : IContentFinder
    {
        public bool TryFindContent(PublishedContentRequest contentRequest)
        {
            if (contentRequest == null)
                return false;

            var url = contentRequest.Uri.AbsolutePath;
            var appCache = ApplicationContext.Current.ApplicationCache.RuntimeCache;

            var blogUrls = appCache.GetCacheItem<Dictionary<string, AubBlogContentFinderItem>>("blogContentFinderCache");
            if (blogUrls != null && blogUrls.ContainsKey(url))
            {
                var content = contentRequest.RoutingContext.UmbracoContext.ContentCache.GetById(
                    blogUrls[url].contentId);

                contentRequest.PublishedContent = content;
                contentRequest.TrySetTemplate(blogUrls[url].templateAlias);
            }

            if (blogUrls == null)
                blogUrls = new Dictionary<string, AubBlogContentFinderItem>();

            var rootNodes = contentRequest.RoutingContext.UmbracoContext.ContentCache.GetAtRoot();

            if (contentRequest.PublishedContent == null)
            {
                var path = contentRequest.Uri.GetAbsolutePathDecoded();
                var parts = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                var content = rootNodes.DescendantsOrSelf(DocTypes.BlogPost)
                    .Where(x => x.UrlName.Equals(parts.Last(), StringComparison.InvariantCultureIgnoreCase)
                            || x.Id.ToString() == parts.Last())
                    .FirstOrDefault();

                if (content != null)
                {
                    var finderItem = new AubBlogContentFinderItem()
                    {
                        contentId = content.Id,
                        templateAlias = content.GetTemplateAlias()
                    };

                    blogUrls.Add(url, finderItem);

                    appCache.InsertCacheItem<Dictionary<string, AubBlogContentFinderItem>>(
                        "blogContentFinderCache", () => blogUrls, priority: CacheItemPriority.Default);

                    contentRequest.PublishedContent = content;
                    contentRequest.TrySetTemplate(finderItem.templateAlias);
                }
                else
                {
                    var blogRoot = rootNodes.DescendantsOrSelf(DocTypes.Blog)
                        .Where(x => parts.Contains(x.UrlName))
                        .FirstOrDefault();

                    if (blogRoot != null)
                    {
                        // we have the blog root, we need to work out the remaining path, and see 
                        // if it matches any of our settings...
                        var blogPlace = parts.IndexOf(blogRoot.UrlName);
                        var blogUrl = "/" + string.Join("/", parts.Skip(blogPlace + 1)) + "/";

                        var item = GetSpecialRoutes(blogRoot, blogUrl);
                        if (item != null)
                        {
                            blogUrls.Add(url, item);

                            appCache.InsertCacheItem<Dictionary<string, AubBlogContentFinderItem>>(
                                "blogContentFinderCache", () => blogUrls, priority: CacheItemPriority.Default);

                            contentRequest.PublishedContent = blogRoot;
                            contentRequest.TrySetTemplate(item.templateAlias);
                        }
                    }

                }
            }

            return contentRequest.PublishedContent != null;
        }


        public AubBlogContentFinderItem GetSpecialRoutes(IPublishedContent blogRoot, string blogUrl)
        {
            Dictionary<string, string> blogRoutes = new Dictionary<string, string>();

            blogRoutes.Add(blogRoot.GetPropertyValue<string>("categoryBase", "/categories/"), "blogCategories");
            blogRoutes.Add(blogRoot.GetPropertyValue<string>("tagBase", "/tags/"), "blogTags");

            foreach (var route in blogRoutes)
            {
                if (blogUrl.StartsWith(route.Key))
                {
                    var finder = new AubBlogContentFinderItem()
                    {
                        contentId = blogRoot.Id,
                        templateAlias = route.Value
                    };

                    return finder;
                }
            }

            return null;
        }


        public class AubBlogContentFinderItem
        {
            public int contentId { get; set; }
            public string templateAlias { get; set; }
        }
    }
}
