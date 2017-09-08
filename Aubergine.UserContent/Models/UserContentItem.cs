using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;

namespace Aubergine.UserContent.Models
{
    public class UserContentItem : IUserContent
    {
        private List<UserContentProperty> _properties
            = new List<UserContentProperty>();

        public UserContentItem() { }

        public UserContentItem(string userContentType)
        {
            UserContentType = userContentType;
        }

        //////////
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

        public Guid NodeKey { get; set; }
        public Guid ParentKey { get; set; }

        public IList<UserContentProperty> Properties
        {
            get { return _properties; }
        }

        public UserContentProperty GetProperty(string alias)
        {
            return _properties != null ?
                _properties.SingleOrDefault(x => x.PropertyAlias.InvariantEquals(alias)) :
                default(UserContentProperty);
        }

        public bool HasProperty(string alias)
        {
            return _properties != null ?
                _properties.Any(x => x.PropertyAlias.InvariantEquals(alias)) : false;
        }

    }
}
