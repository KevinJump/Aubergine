using System;
using System.Collections.Generic;
using Aubergine.UserContent.Models;
using Umbraco.Core;

namespace Aubergine.UserContent
{
    public interface IUserContentService
    {
        void ClearCacheByPageKey(Guid key);
        Attempt<IUserContent> Delete(IUserContent content);
        IUserContent Get(Guid key);
        IUserContent Get(int id);
        IEnumerable<IUserContent> GetApprovedUserContent(Guid contentKey);
        IEnumerable<IUserContent> GetApprovedUserContent(Guid contentKey, string userContentType);
        IEnumerable<IUserContent> GetChildren(Guid key, bool getAll = false);
        int GetContentCount(Guid key);
        int GetChildCount(Guid key);
        IEnumerable<IUserContent> GetUserContent(Guid contentKey, bool getAll);
        IEnumerable<IUserContent> GetUserContent(Guid contentKey, UserContentStatus status, bool getAll, string userContentType = "");
        Attempt<IUserContent> Save(IUserContent content);
        Attempt<int> UpdateStatus(Guid key, UserContentStatus status);
    }
}