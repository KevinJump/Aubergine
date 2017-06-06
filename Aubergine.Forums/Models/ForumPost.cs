using Aubergine.UserContent;
using Aubergine.UserContent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace Aubergine.Forums.Models
{
    /// <summary>
    ///  forum post, example of how you can use the UserContentItem
    ///  to store and get you shizzle.
    /// </summary>
    public class ForumPost : UserContentItem
    {
        public int Level
        {
            get { return this.GetPropertyValue<int>("level", -1); }
            set { this.SetProperty<int>("level", value); }
        }

        public string Body
        {
            get { return this.GetPropertyValue<string>("body", string.Empty); }
            set { this.SetProperty<string>("body", value); }
        }
    }

    public class ForumInfo
    {
        public IPublishedContent Page { get; set; }
        public IEnumerable<ForumPost> Posts { get; set; }
    }
}
