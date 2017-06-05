using Aubergine.UserContent;
using Aubergine.UserContent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Aubergine.Comments
{
    public static class CommentPublishedContentExtensions
    {
        public static IEnumerable<IUserContent> GetComments(this IPublishedContent content)
        {
            return content.GetUserContent(Comments.UserContentTypeAlias);
        }

        public static Attempt<IUserContent> AddComment(this IPublishedContent content, IUserContent comment)
        {
            comment.NodeKey = content.GetKey();
            return UserContentContext.Current.Instances[Comments.Instance].Service.Save(comment);
        }
    } 
}
