using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Aubergine.Core;
using Semver;
using Umbraco.Core;
using Umbraco.Web.Routing;
using Umbraco.Core.Services;
using Umbraco.Core.Cache;

namespace Aubergine.Blog
{
    public class AubergineBlog : ApplicationEventHandler, IAubergineExtension
    {
        private readonly SemVersion targetVersion = new SemVersion(1, 0, 0);

        public string Name => "Blogs";
        public string ExtensionId => "{22310F65-948B-45AC-A2C3-4CA645F17B33}";
        public string Version => targetVersion.ToString();


        private IRuntimeCacheProvider cache;
        int blogPostContentTypeId;

        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            UrlProviderResolver.Current.InsertTypeBefore<DefaultUrlProvider, BlogUrlProvider>();
            ContentFinderResolver.Current.InsertTypeBefore<ContentFinderByNotFoundHandlers, BlogContentFinder>();

            cache = applicationContext.ApplicationCache.RuntimeCache;
        }

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ContentService.Published += ContentService_Published;

            var blogPostContentType = applicationContext.Services.ContentTypeService.GetContentType(DocTypes.BlogPost);
            if (blogPostContentType != null)
                blogPostContentTypeId = blogPostContentType.Id;

            var m = new Aubergine.Core.Migrations.MigrationManager(applicationContext);
            m.ApplyMigration(Name, targetVersion);

        }

        private void ContentService_Published(Umbraco.Core.Publishing.IPublishingStrategy sender, Umbraco.Core.Events.PublishEventArgs<Umbraco.Core.Models.IContent> e)
        {
            if (blogPostContentTypeId != -1 && e.PublishedEntities.Any(x => x.ContentTypeId == blogPostContentTypeId))
            {
                // when a blog post is published, we clear the 
                // content finder cache, so that the url's for
                // blog posts are cleared. 
                cache.ClearCacheItem("blogContentFinderCache");
            }
        }
    }

    public static class DocTypes
    {
        public static string BlogPost = "blogPost";
        public static string Blog = "blog";
        public static string BlogPosts = "blogPosts";
    }

    public static class Properties
    {
        public static string Permalinks = "permalinks";
        public static string PublishDate = "publicationDate";
        public static string PostsPerPage = "postsPerPage";

        public static string Tags = "tags";
        public static string Categories = "categories";

        /// <summary>base url for tags </summary>
        public static string TagBase = "tagBase";
    }

}
