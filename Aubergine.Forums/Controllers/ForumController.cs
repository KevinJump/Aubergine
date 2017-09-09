using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Aubergine.Forums.Models;
using Aubergine.UserContent;
using Aubergine.UserContent.Models;
using Aubergine.UserContent.Services;
using AutoMapper;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace Aubergine.Forums
{
    [PluginController("AubergineForums")]
    public class ForumsController : SurfaceController
    {
        private readonly IUserContentService userContentService;

        public ForumsController()
        {
            userContentService = UserContentContext.Current
                .Instances[Forums.Instance].Service;
        }

        [ChildActionOnly]
        public ActionResult ListPosts(Guid contentKey)
        {
            // use the cache - this will be quicker
            var items = Umbraco.GetUserContent(contentKey, Forums.UserContentTypeAlias, Forums.Instance);

            var forumSummary = new ForumSummaryInfo();

            // top level posts in the forum
            var posts = Mapper.Map<IEnumerable<ForumPost>>
                (items.Where(x => x.ParentKey == Guid.Empty));

            foreach (var post in posts)
            {
                forumSummary.Posts.Add(new PostSummaryInfo
                {
                    Name = post.Name,
                    Author = post.Author,
                    Id = post.Id,
                    LastEditDate = post.UpdatedDate,
                    Replies = userContentService.GetChildCount(post.Key)
                });
            }

            forumSummary.Page = CurrentPage;

            return PartialView("posts", forumSummary);
        }

        [ChildActionOnly]
        public ActionResult ShowThread(Guid postKey)
        {
            var items = new List<IUserContent>();
            var item = userContentService.Get(postKey);
            if (item != null) {

                items.Add(item);
                items.AddRange(userContentService.GetChildren(item.Key, false));

                var forumInfo = new ForumInfo
                {
                    Posts = Mapper.Map<IEnumerable<ForumPost>>(items),
                    Page = CurrentPage
                };

                return PartialView("thread", forumInfo);
            }

            return new HttpNotFoundResult();
        }

        [MemberAuthorize]
        [ChildActionOnly]
        public ActionResult AddPost(Guid contentKey, Guid? parentKey = null, bool showTitle = false)
        {
            var member = Members.GetCurrentMember();

            var model = new ForumPostViewModel
            {
                ContentKey = contentKey,
                AuthorKey = member.GetKey(),
                ShowTitle = showTitle,
            };

            if (parentKey != null)
            {
                model.ParentKey = parentKey.Value;
            }

            return PartialView("newpost", model);
        }

        [MemberAuthorize]
        [NotChildAction]
        [ValidateAntiForgeryToken]
        public ActionResult AddPost(ForumPostViewModel model)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            var member = Members.GetCurrentMember();

            LogHelper.Debug<ForumsController>("Body: {0}", () => model.Body);

            var post = new ForumPost
            {
                NodeKey = model.ContentKey,
                Body = model.Body.SanitizeHtml(),
                Name = model.Name ?? "",
                Status = UserContentStatus.Approved,
                UserContentType = Forums.UserContentTypeAlias,
                Level = 1,
                AuthorId = member.GetKey().ToString(),
                Author = member.Name,
                Answer = false,
                UpVotes = 0,
                DownVotes = 0
            };

            if (model.ParentKey != null)
                post.ParentKey = model.ParentKey;

            var attempt = userContentService.Save(post);
            if (!attempt.Success)
            {
                ModelState.AddModelError("", "Can't add post");
            }

            return RedirectToCurrentUmbracoUrl();
        }
    }
}
