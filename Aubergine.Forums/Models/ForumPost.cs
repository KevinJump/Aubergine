using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aubergine.UserContent;
using Aubergine.UserContent.Models;
using Umbraco.Core.Models;

namespace Aubergine.Forums.Models
{
    public partial class ForumPost : UserContentItem
    {
        public string Body
        {
            get { return this.GetPropertyValue<string>("body", string.Empty); }
            set { this.SetProperty<string>("body", value); }
        }

        public int Level { get; set; }
        public bool Answer { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
    }

    public class ForumInfo
    {
        public IPublishedContent Page { get; set; }
        public IEnumerable<ForumPost> Posts { get; set; }
    }
}
