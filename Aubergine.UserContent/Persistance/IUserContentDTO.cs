using System;

namespace Aubergine.UserContent.Persistance
{
    public interface IUserContentDTO
    {
        string Author { get; set; }
        string AuthorId { get; set; }
        DateTime CreatedDate { get; set; }
        int id { get; set; }
        Guid Key { get; set; }
        string Name { get; set; }
        Guid? NodeKey { get; set; }
        Guid? ParentKey { get; set; }
        string PropertyData { get; set; }
        int Status { get; set; }
        DateTime UpdatedDate { get; set; }
        string Url { get; set; }
        string UserContentType { get; set; }
    }
}