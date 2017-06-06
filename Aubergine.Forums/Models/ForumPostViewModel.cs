using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using UmbracoValidationAttributes;

namespace Aubergine.Forums.Models
{
    public class ForumPostViewModel
    {
        public bool ShowTitle { get; set; }

        public Guid Key { get; set; }
        public Guid ContentKey { get; set; }
        public Guid ParentKey { get; set; }

        [UmbracoDisplayName("Aub.Forums.Title")]
        public string Name { get; set; }

        [AllowHtml]
        public string Body { get; set; }

        public Guid AuthorKey { get; set; }
    }
}
