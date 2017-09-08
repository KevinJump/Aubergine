using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Aubergine.UserContent;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Routing;

namespace Aubergine.Forums
{
    /// <summary>
    ///  forum content finder. 
    ///  
    ///  allows us to have forums posts take the form 
    ///  http://sitename/path/to/forum/{id}-title/ 
    ///  
    /// </summary>
    public class ForumContentFinder : IContentFinder
    {
        public bool TryFindContent(PublishedContentRequest contentRequest)
        {
            if (contentRequest == null)
                return false;

            var url = contentRequest.Uri.GetAbsolutePathDecoded();
            var urlParts = url.ToDelimitedList("/");

            if (urlParts.Count > 1)
            {
                var forumName = urlParts[urlParts.Count - 2];
                var rootNodes = contentRequest.RoutingContext.UmbracoContext
                    .ContentCache.GetAtRoot();

                // make this a cache thing (for speed!)
                var forums = rootNodes.DescendantsOrSelf("forum");

                var forumContent = forums.Where(x => x.UrlName.InvariantEquals(forumName))
                    .FirstOrDefault();

                if (forumContent != null)
                {
                    var postParts = urlParts.Last().ToDelimitedList("-");
                    if (postParts.Count > 1)
                    {
                        var postId = postParts.First();
                        if (int.TryParse(postId, out int id))
                        {
                            var service = UserContentContext.Current.Instances[AubergineForums.Instance]
                                .Service;

                            var post = service.Get(id);
                            if (post == null)
                                return false;

                            var context =
                                contentRequest.RoutingContext.UmbracoContext.HttpContext.Items["postKey"] = post.Key;
                        }
                    }

                    contentRequest.PublishedContent = forumContent;
                    contentRequest.TrySetTemplate("forumThread");
                }
            }
            return contentRequest.PublishedContent != null;
        }
    }
}
