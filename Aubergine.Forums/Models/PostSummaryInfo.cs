using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace Aubergine.Forums.Models
{
    public class ForumSummaryInfo
    {
        public IPublishedContent Page { get; set; }
        public List<PostSummaryInfo> Posts { get; set; }

        public ForumSummaryInfo()
        {
            Posts = new List<PostSummaryInfo>();
        }
    }

    public class PostSummaryInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public DateTime LastEditDate { get; set; }

        public int Replies { get; set; }
        public string Url()
        {
            return Id + "-" + Name.ToSafeFileName();
        }
    }
}
