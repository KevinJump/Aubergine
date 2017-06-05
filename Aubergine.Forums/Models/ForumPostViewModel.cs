using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aubergine.Forums.Models
{
    public class ForumPostViewModel
    {
        public Guid Key { get; set; }
        public Guid ContentKey { get; set; }
        public Guid ParentKey { get; set; }

        public string Name { get; set; }

        public string Body { get; set; }

        public Guid AuthorKey { get; set; }
    }
}
