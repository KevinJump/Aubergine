using System;
using System.Collections.Generic;
using Aubergine.UserContent.Models;

namespace Aubergine.UserContent.Persistance
{
    public interface IUserContentRepository
    {
        IUserContent Create(IUserContent entity);
        void Delete(Guid key);
        IUserContent Get(Guid key);

        IEnumerable<IUserContent> GetByContentId(Guid contentKey, bool getAll = false);
        IEnumerable<IUserContent> GetByContentId(
            Guid contentKey, 
            UserContentStatus status = UserContentStatus.Approved, 
            bool getAll = false,
            string contentType = "");

        IEnumerable<IUserContent> GetChildren(Guid key, bool getAll = false);
        IEnumerable<IUserContent> GetChildren(
            Guid key, 
            UserContentStatus status = UserContentStatus.Approved, 
            bool getAll = false,
            string contentType = "");

        IUserContent Update(IUserContent entity);

        bool SetStatus(Guid key, UserContentStatus status);
    }
}