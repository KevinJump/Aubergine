using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aubergine.Core;
using Aubergine.Core.Migrations;
using Umbraco.Core;
using Umbraco.Core.Cache;
using Umbraco.Core.Services;
using Umbraco.Web.Routing;

namespace Aubergine.Blog
{
    public class AubergineBlog : ApplicationEventHandler, IAubergineExtension
    {
        private IRuntimeCacheProvider cache;
        private int blogPostContentTypeId;

        public string Name => Blog.ProductName;
        public string ExtensionId => "{22310F65-948B-45AC-A2C3-4CA645F17B33}";
        public string Version => Blog.ProductVersion;
        public string ProductName => Blog.ProductName;

        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            UrlProviderResolver.Current.InsertTypeBefore<DefaultUrlProvider, BlogUrlProvider>();
            ContentFinderResolver.Current.InsertTypeBefore<ContentFinderByNotFoundHandlers, BlogContentFinder>();
        }

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {

            var blogPostContentType = applicationContext.Services.ContentTypeService.GetContentType(Blog.Presets.DocTypes.BlogPost);
            if (blogPostContentType != null)
            {
                blogPostContentTypeId = blogPostContentType.Id;

                cache = applicationContext.ApplicationCache.RuntimeCache;
                ContentService.Published += ContentService_Published;

            }
        }

        private void ContentService_Published(Umbraco.Core.Publishing.IPublishingStrategy sender, Umbraco.Core.Events.PublishEventArgs<Umbraco.Core.Models.IContent> e)
        {
            if (blogPostContentTypeId != -1 && e.PublishedEntities.Any(x => x.ContentTypeId == blogPostContentTypeId))
            {
                foreach (var item in e.PublishedEntities)
                {
                    cache.ClearCacheByKeySearch("ablog_");
                }
            }
        }
    }


    public class Blog
    {
        public const string ProductName = "AubergineBlogs";
        public const string ProductVersion = "1.0.0";

        public class Presets
        {
            public class DocTypes
            {
                public const string BlogPost = "blogPost";
                public const string Blog = "blog";
                public const string BlogPosts = "blogPosts";
            }

            public class Properties
            {
                public const string Permalinks = "permalinks";
                public const string PublishDate = "publicationDate";
                public const string PostsPerPage = "postsPerPage";
                public const string Tags = "tags";
                public const string Categories = "categories";
                public const string TagBase = "tabBase";
            }
        }
    }
}
