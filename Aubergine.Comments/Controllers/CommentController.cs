using Aubergine.Comments.Models;
using Aubergine.UserContent;
using Aubergine.UserContent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Core.Logging;

namespace Aubergine.Comments
{
    [PluginController("Aubergine")]
    public class CommentController : SurfaceController
    {
        [ChildActionOnly]
        public ActionResult Comment()
        {
            CommentViewModel comment = new CommentViewModel();
            return PartialView(Comments.Views.CommentBox, comment);   
        }

        [HttpPost]
        [NotChildAction]
        [ValidateAntiForgeryToken]
        public ActionResult Comment(CommentViewModel model)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            var comment = new UserContentItem(Comments.UserContentTypeAlias)
            {
                Name = "comment" + DateTime.Now.ToString("yyyyMMddhhmmss"),
                NodeKey = CurrentPage.GetKey(),
                Status = UserContentStatus.Approved
            };

            comment.SetProperty<string>(Comments.Properties.Email, model.EmailAddress);
            comment.SetProperty<string>(Comments.Properties.Name, model.Name);
            comment.SetProperty<string>(Comments.Properties.Comment, model.Comment);

            var item = Umbraco.SaveUserContent(comment, Comments.Instance);
            Logger.Info<CommentController>("User Comment Saved: {0} {1} [{2}]", () => item.Success, () => item.Result.Author, () => CurrentPage.GetKey());

            return RedirectToCurrentUmbracoPage();
        }
    }
}
