using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aubergine.UserContent.Models;

namespace Aubergine.Comments.Models
{
    public class PageCommentsViewModel
    {
        public IEnumerable<IUserContent> Comments { get; set; }
        public bool AllowComments { get; set; }
    }
}
