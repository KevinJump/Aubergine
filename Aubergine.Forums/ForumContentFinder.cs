using Aubergine.UserContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Routing;

namespace Aubergine.Forums
{
    public class ForumContentFinder : IContentFinder
    {
        public bool TryFindContent(PublishedContentRequest contentRequest)
        {
            if (contentRequest == null)
                return false;

            var url = contentRequest.Uri.GetAbsolutePathDecoded();
            var parts = url.ToDelimitedList("/");
            // the last part should be the forum post.
            // the part before is the page/forum it's

            if (parts.Count > 1)
            {
                var forumName = parts[parts.Count - 2];
                var rootNodes = contentRequest.RoutingContext.UmbracoContext.ContentCache.GetAtRoot();

                var forum = rootNodes.DescendantsOrSelf("forum")
                    .Where(x => x.UrlName.Equals(forumName, StringComparison.InvariantCultureIgnoreCase))
                    .FirstOrDefault();

                if (forum != null)
                {
                    // we have the parent forum, now we can get the post by the id
                    var postParts = parts.Last().ToDelimitedList("-");
                    if (postParts.Count > 1)
                    {
                        var postId = postParts.First();
                        int id;
                        if (int.TryParse(postId, out id))
                        {
                            var userContentService = UserContentContext.Current.Instances[Product.Instance].Service;
                            var post = userContentService.Get(id);
                            if (post == null)
                                return false; 

                            HttpContext.Current.Items["postKey"] = post.Key;
                        }
                    }

                    contentRequest.PublishedContent = forum;
                    contentRequest.TrySetTemplate("forumThread");
                }
            }

            return contentRequest.PublishedContent != null;

        }
    }
}
