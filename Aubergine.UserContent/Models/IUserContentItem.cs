using System;
using System.Collections.Generic;

namespace Aubergine.UserContent.Models
{
    public interface IUserContent
    {
        string Author { get; set; }
        string AuthorId { get; set; }
        DateTime CreatedDate { get; set; }
        int Id { get; set; }
        Guid Key { get; set; }
        string Name { get; set; }
        Guid NodeKey { get; set; }
        Guid ParentKey { get; set; }
        IList<UserContentProperty> Properties { get; }
        UserContentStatus Status { get; set; }
        DateTime UpdatedDate { get; set; }
        string Url { get; set; }
        string UserContentType { get; set; }

        UserContentProperty GetProperty(string alias);
        bool HasProperty(string alias);
    }
}