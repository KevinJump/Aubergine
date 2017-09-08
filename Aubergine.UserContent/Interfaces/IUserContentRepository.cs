using System;
using System.Collections.Generic;
using Aubergine.UserContent.Models;

namespace Aubergine.UserContent
{
    public interface IUserContentRepository
    {
        IUserContent Create(IUserContent entity);
        void Delete(Guid key);
        IUserContent Get(Guid key);
        IUserContent Get(int id);
        IEnumerable<IUserContent> GetByContentKey(Guid contentKey, UserContentStatus status = UserContentStatus.Approved, bool getAll = false, string contentType = "");
        IEnumerable<IUserContent> GetChildren(Guid key, UserContentStatus status = UserContentStatus.Approved, bool getAll = false, string contentType = "");
        int GetContentCount(Guid key);
        int GetChildCount(Guid key);
        int SetStatus(Guid key, UserContentStatus status);
        IUserContent Update(IUserContent entity);
    }
}