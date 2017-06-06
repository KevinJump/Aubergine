using Aubergine.UserContent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace Aubergine.UserContent
{
    /// <summary>
    ///  User Content item - a basic element of user generated content.
    /// </summary>
    public class UserContentItem : IUserContent
    {
        private List<UserContentProperty> _properties;

        public UserContentItem()
        {
            _properties = new List<UserContentProperty>();
        }

        public UserContentItem(string userContentType)
        {
            UserContentType = userContentType;
            _properties = new List<UserContentProperty>();
        }

        public UserContentItem(IEnumerable<UserContentProperty> properties)
        {
            _properties = properties.ToList();
        }


        public int Id { get; set; }


        public Guid Key { get; set; }
        public string UserContentType { get; set; }

        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public string Author { get; set; }
        public string AuthorId { get; set; }

        public string Url { get; set; }

        public UserContentStatus Status { get; set; }

        /// <summary>
        ///  Key of node this content belongs too...
        /// </summary>
        public Guid NodeKey { get; set; }

        public Guid ParentKey { get; set; }

        public IList<UserContentProperty> Properties
        {
            get { return _properties; }
        }

        public IEnumerable<IUserContent> Children { get; }


        public UserContentProperty GetProperty(string alias)
        {
            return _properties != null ?
                _properties.SingleOrDefault(x => x.PropertyAlias == alias) :
                default(UserContentProperty);
        }

        public bool HasProperty(string alias)
        {
            return
                _properties != null ?
                _properties.Any(x => x.PropertyAlias == alias) : false;
        }
    }

}
