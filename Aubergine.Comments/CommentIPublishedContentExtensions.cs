using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aubergine.UserContent.Models;
using Umbraco.Core.Models;
using Aubergine.UserContent;
using Umbraco.Core;
using Umbraco.Web;

namespace Aubergine.Comments
{
    public static class CommentIPublishedContentExtensions
    {
        public static IEnumerable<IUserContent> GetComments(this IPublishedContent content)
        {
            return content.GetUserContent(Comments.UserContentType, Comments.Instance);
        }

        public static Attempt<IUserContent> AddComment(this IPublishedContent content, IUserContent comment)
        {
            comment.NodeKey = content.GetKey();
            return UserContentContext.Current.Instances[Comments.Instance].Service.Save(comment);
        }
    }
}
