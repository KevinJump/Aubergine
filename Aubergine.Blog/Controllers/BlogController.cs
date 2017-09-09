using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace Aubergine.Blog.Controllers
{
    [PluginController("AubergineBlog")]
    public class BlogController : SurfaceController
    {
        [ChildActionOnly]
        public ActionResult Post(IPublishedContent content)
        {
            return PartialView("post", content);
        }

        [ChildActionOnly]
        public ActionResult ListPosts(IPublishedContent content)
        {
            var postsPerPage = content.GetPropertyValue<int>
                (Blog.Presets.Properties.PostsPerPage, true, 20);

            var posts = content.Descendants(Blog.Presets.DocTypes.BlogPost)
                .Where(x => x.IsVisible())
                .OrderByDescending(x =>
                    x.GetPropertyValue<DateTime>(Blog.Presets.Properties.PublishDate, x.CreateDate));

            return PartialView("listPosts", posts);
        }

        [ChildActionOnly]
        public ActionResult BlogHeader(IPublishedContent content)
        {
            return PartialView("header", content);
        }

    }
}
