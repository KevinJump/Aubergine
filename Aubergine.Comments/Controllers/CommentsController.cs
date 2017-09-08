using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Aubergine.Comments.Models;
using Aubergine.UserContent;
using Aubergine.UserContent.Models;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace Aubergine.Comments
{
    [PluginController("AubergineComments")]
    public class CommentsController : SurfaceController
    {
        [ChildActionOnly]
        public ActionResult ShowComments()
        {
            var commentsView = new PageCommentsViewModel
            {
                AllowComments = CurrentPage.GetPropertyValue("allowComments", false, false),
                Comments = Umbraco.GetUserContent(CurrentPage.GetKey(), Comments.UserContentType, Comments.Instance)
            };

            return PartialView("ShowComments", commentsView);
        }

        [ChildActionOnly]
        public ActionResult Comment()
        {
            CommentViewModel comment = new CommentViewModel();
            comment.Status = UserContentStatus.Pending;
            return PartialView("CommentBox", comment);
        }

        [HttpPost]
        [NotChildAction]
        [ValidateAntiForgeryToken]
        public ActionResult Comment(CommentViewModel model)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            var comment = new UserContentItem("comment")
            {
                Name = $"comment {DateTime.Now.ToString("yyyyMMddhhmmss")}",
                NodeKey = CurrentPage.GetKey(),
                Status = UserContentStatus.Approved,
                Author = model.Name,
                AuthorId = model.EmailAddress,
                UserContentType = Comments.UserContentType
               
            };

            comment.SetProperty<string>("email", model.EmailAddress);
            comment.SetProperty<string>("name", model.Name);
            comment.SetProperty<string>("comment", model.Comment.SanitizeHtml());

            var item = Umbraco.SaveUserContent(comment, Comments.Instance);

            return RedirectToCurrentUmbracoPage();
        }
    }
}
