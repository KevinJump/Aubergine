using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Aubergine.UserContent.Models;

namespace Aubergine.Comments.Models
{
    public class CommentViewModel
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }

        [AllowHtml]
        public string Comment { get; set; }

        public UserContentStatus Status { get; set; }
    }
}
