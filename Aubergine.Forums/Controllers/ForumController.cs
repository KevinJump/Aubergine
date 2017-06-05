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
            var posts = Mapper.Map<IEnumerable<ForumPost>>(items);
            return PartialView("forums/posts", posts);
        }


        [MemberAuthorize]
        [ChildActionOnly]
        public ActionResult AddPost(Guid contentKey)
        {
            var member = Members.GetCurrentMember();

            var model = new ForumPostViewModel
            {
                ContentKey = contentKey, 
                AuthorKey = member.GetKey(),
            };
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
                Name = model.Name,
                Status = UserContentStatus.Approved,
                UserContentType = Product.UserContentTypeAlias,
                Level = 1,
                AuthorId = member.GetKey().ToString(),
                Author = member.Name
            };

            // var userContent = Mapper.Map<UserContentItem>(post);
            var attempt = _userContentService.Save(post);
            if (!attempt.Success)
            {
                ModelState.AddModelError("", "Can't add post - some error ");
            }

            return RedirectToCurrentUmbracoPage();

        }


    }
}
