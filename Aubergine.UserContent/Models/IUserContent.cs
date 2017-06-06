using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace Aubergine.UserContent.Models
{
    /// <summary>
    ///  Interface for user content, at the front / back end. 
    /// </summary>
    public interface IUserContent
    {
        /// <summary>
        /// The internal ID for an item - where possible we 
        /// use the key to make things more portable
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// unique ID for this piece of user content
        /// </summary>
        Guid Key { get; set; }

        /// <summary>
        /// string identifer for this type of content
        /// </summary>
        string UserContentType { get; set; }

        /// <summary>
        /// name of content piece
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// unique URL to identify this bit of content
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// date of creation
        /// </summary>
        DateTime CreatedDate { get; set; }

        /// <summary>
        /// date of last update
        /// </summary>
        DateTime UpdatedDate { get; set; }

        /// <summary>
        /// identifing name of content author
        /// </summary>
        string Author { get; set; }

        /// <summary>
        /// identifing id of content author
        /// </summary>
        string AuthorId { get; set; }

        /// <summary>
        /// status of content item
        /// </summary>
        UserContentStatus Status { get; set; }

        /// <summary>
        /// Guid of Umbraco Conttent node.
        /// </summary>
        Guid NodeKey { get; set; }

        /// <summary>
        /// Guid of parent (for hieracical content)
        /// </summary>
        Guid ParentKey { get; set; }
        
        /// <summary>
        /// Child user content
        /// </summary>
        IEnumerable<IUserContent> Children { get; }

        /// <summary>
        /// collecion of user properties
        /// </summary>
        IList<UserContentProperty> Properties { get; }

        /// <summary>
        /// get a property from the content
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        UserContentProperty GetProperty(string alias);

        /// <summary>
        /// returns true if content has property.
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        bool HasProperty(string alias);
        
    }
}
