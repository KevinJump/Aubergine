using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using System.Web.Caching;
using Umbraco.Core.Cache;
using Umbraco.Web.Routing;
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

            var cacheKey = $"ablog_{url}";

            var postInfo = appCache.GetCacheItem<AubBlogContentFinderItem>(cacheKey);
            if (postInfo != null)
            {
                var postContent = contentRequest.RoutingContext.UmbracoContext
                    .ContentCache.GetById(postInfo.ContentId);

                contentRequest.PublishedContent = postContent;
                contentRequest.TrySetTemplate(postInfo.TemplateAlias);
                return true; 
            }

            var rootNodes = contentRequest.RoutingContext.UmbracoContext
                    .ContentCache.GetAtRoot();

            var path = contentRequest.Uri.GetAbsolutePathDecoded();
            var parts = path.ToDelimitedList("/");

            // get the content by the post-name or the id. 
            var content = rootNodes.DescendantsOrSelf(Blog.Presets.DocTypes.BlogPost)
                    .Where(x => x.UrlName.InvariantEquals(parts.Last()) || x.Id.ToString() == parts.Last())
                    .FirstOrDefault();

            if (content != null)
            {
                var finderItem = new AubBlogContentFinderItem
                {
                    ContentId = content.Id,
                    TemplateAlias = content.GetTemplateAlias()
                };

                appCache.InsertCacheItem<AubBlogContentFinderItem>(
                    cacheKey, () => finderItem, priority: CacheItemPriority.Default);

                contentRequest.PublishedContent = content;
                contentRequest.TrySetTemplate(finderItem.TemplateAlias);
            }
            else
            {
                // the slower way - we need to work out by blog root what the post
                // is (using the post settings)
                var blogRoot = rootNodes.DescendantsOrSelf(Blog.Presets.DocTypes.BlogPosts)
                    .Where(x => parts.InvariantContains(x.UrlName))
                    .FirstOrDefault();

                if (blogRoot != null)
                {
                    var blogPlace = parts.IndexOf(blogRoot.UrlName);
                    var blogUrl = $"/{string.Join("/", parts.Skip(blogPlace + 1))}/";

                    // goes and gets the special (category or tag) route. 
                    var item = GetSpecialRoute(blogRoot, blogUrl);
                    if (item != null)
                    {
                        appCache.InsertCacheItem<AubBlogContentFinderItem>(
                            cacheKey, () => item, priority: CacheItemPriority.Default);

                        contentRequest.PublishedContent = blogRoot;
                        contentRequest.TrySetTemplate(item.TemplateAlias);
                    }
                }
            }

            return contentRequest.PublishedContent != null;
        }

        private AubBlogContentFinderItem GetSpecialRoute(IPublishedContent blog, string blogUrl)
        {
            Dictionary<string, string> routes = new Dictionary<string, string>();

            routes.Add(blog.GetPropertyValue<string>("categoryBase", "/categories/"), "blogCategories");
            routes.Add(blog.GetPropertyValue<string>("tagBase", "/tags/"), "blogTags");

            foreach(var route in routes)
            {
                if (blogUrl.InvariantStartsWith(route.Key))
                {
                    var finder = new AubBlogContentFinderItem
                    {
                        ContentId = blog.Id,
                        TemplateAlias = route.Value
                    };

                    return finder;
                }
            }

            return null;
        }

        private class AubBlogContentFinderItem
        {
            public int ContentId { get; set; }
            public string TemplateAlias { get; set; }

        }

    }
}
