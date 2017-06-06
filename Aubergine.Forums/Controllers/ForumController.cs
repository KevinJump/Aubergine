using Aubergine.Forums.Models;
using AutoMapper;
using Aubergine.UserContent;
using Aubergine.UserContent.Models;
using Aubergine.UserContent.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Core;

namespace Aubergine.Forums
{
    [PluginController("Aubergine")]
    public class ForumController : SurfaceController
    {
        private readonly IUserContentService _userContentService;
        public ForumController()
        {
            _userContentService = UserContentContext.Current.Instances[Product.Instance].Service;
        }

        [ChildActionOnly]
        public ActionResult ListPosts(Guid contentKey)
        {
            var items = _userContentService.GetByContentKey(contentKey, getAll: false);

            var forumInfo = new ForumSummaryInfo();
            var posts = Mapper.Map<IEnumerable<ForumPost>>(items.Where(x => x.ParentKey == Guid.Empty));
            foreach (var post in posts)
            {
                forumInfo.Posts.Add(new PostSummaryInfo
                {
                    Name = post.Name,
                    Author = post.Author,
                    Id = post.Id,
                    LastEditDate = post.UpdatedDate,
                    Replies = _userContentService.GetChildren(post.Key, false).Count()
                });
            }

            forumInfo.Page = CurrentPage;

            return PartialView("forums/posts", forumInfo);
        }

        [ChildActionOnly]
        public ActionResult ShowThread(Guid postKey)
        {
            var items = new List<IUserContent>();
            var item = _userContentService.Get(postKey);
            if (item != null)
            {
                items.Add(item);
                items.AddRange(_userContentService.GetChildren(item.Key, false));

                var forumInfo = new ForumInfo
                {
                    Posts = Mapper.Map<IEnumerable<ForumPost>>(items),
                    Page = CurrentPage
                };
                return PartialView("forums/thread", forumInfo);
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
                ShowTitle = showTitle
            };

            if (parentKey != null)
            {
                model.ParentKey = parentKey.Value;
            }
            return PartialView("forums/newpost", model);
        }

        [MemberAuthorize]
        [NotChildAction]
        [ValidateAntiForgeryToken]
        public ActionResult AddPost(ForumPostViewModel model)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            var member = Members.GetCurrentMember();

            var post = new ForumPost
            {
                NodeKey = model.ContentKey,
                Body = model.Body,
                Name = model.Name.IsNullOrWhiteSpace() ? "" : model.Name,
                Status = UserContentStatus.Approved,
                UserContentType = Product.UserContentTypeAlias,
                Level = 1,
                AuthorId = member.GetKey().ToString(),
                Author = member.Name,
            };

            if (model.ParentKey != null)
            {
                post.ParentKey = model.ParentKey;
            }

            // var userContent = Mapper.Map<UserContentItem>(post);
            var attempt = _userContentService.Save(post);
            if (!attempt.Success)
            {
                ModelState.AddModelError("", "Can't add post - some error ");
            }

            return RedirectToCurrentUmbracoUrl();

        }


    }
}
