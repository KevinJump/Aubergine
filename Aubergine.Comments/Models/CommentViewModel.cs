using Aubergine.UserContent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aubergine.Comments.Models
{
    public class CommentViewModel
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Comment { get; set; }

        public UserContentStatus Status { get; set; }
    }
}
