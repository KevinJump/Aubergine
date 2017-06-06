using Aubergine.UserContent.Cache;
using Aubergine.UserContent.Models;
using Aubergine.UserContent.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Events;

namespace Aubergine.UserContent.Services
{
    public class UserContentService : IUserContentService
    {
        IUserContentRepository _userRepo;
        UserContentCacheRefresher _cacheRefresher;

        public UserContentService(
            IUserContentRepository userRepo,
            UserContentCacheRefresher cacheRefresher)
        {
            _userRepo = userRepo;
            _cacheRefresher = cacheRefresher;
        }

        public IUserContent Get(Guid key)
        {
            return _userRepo.Get(key);
        }

        public IUserContent Get(int id)
        {
            return _userRepo.Get(id);
        }

        public IEnumerable<IUserContent> GetByContentKey(Guid contentKey, bool getAll = false)
        {
            return GetByContentKey(contentKey, UserContentStatus.Approved, getAll);
        }

        public IEnumerable<IUserContent> GetByContentKey(Guid contentKey, UserContentStatus status = UserContentStatus.Approved, bool getAll = false)
        {
            return _userRepo.GetByContentId(contentKey, status, getAll);
        }

        public IEnumerable<IUserContent> GetChildren(Guid key, bool getAll = false)
        {
            return GetChildren(key, UserContentStatus.Approved, getAll);
        }
        public IEnumerable<IUserContent> GetChildren(Guid key, UserContentStatus status = UserContentStatus.Approved, bool getAll = false)
        {
            return _userRepo.GetChildren(key, status, getAll);
        }

        // return attempt. 
        public Attempt<IUserContent> Save(IUserContent content)
        {
            if (Saving.IsRaisedEventCancelled(
                new SaveEventArgs<IUserContent>(content, true), this))
            {
                return Attempt.Fail<IUserContent>(content, new Exception("Blocked by Event"));
            }

            if (content.ParentKey == null)
                content.ParentKey = Guid.Empty;

            if (content.Key != null && content.Key != Guid.Empty)
            {
                var existing = _userRepo.Get(content.Key);
                if (existing != null)
                {
                    existing.UpdatedDate = DateTime.Now;
                    var item = _userRepo.Update(content);
                    return Attempt.Succeed<IUserContent>(item);
                }
            }

            content.Key = Guid.NewGuid();
            content.CreatedDate = content.UpdatedDate = DateTime.Now;

            var obj = _userRepo.Create(content);

            _cacheRefresher.RefreshAll();

            Saved.RaiseEvent(new SaveEventArgs<IUserContent>(obj), this);

            return Attempt.Succeed<IUserContent>(obj);
        }

        public Attempt<IUserContent> Delete(IUserContent content)
        {
            if (Deleting.IsRaisedEventCancelled(new DeleteEventArgs<IUserContent>(content, true), this))
            {
                return Attempt.Fail<IUserContent>(content, new Exception("Blocked by event"));
            }

            _userRepo.Delete(content.Key);

            _cacheRefresher.Refresh(content.Key);

            Deleted.RaiseEvent(new DeleteEventArgs<IUserContent>(content), this);

            return Attempt.Succeed<IUserContent>(content);
        }

        public bool SetStatus(Guid key, UserContentStatus status)
        {
            _cacheRefresher.RefreshAll();
            return _userRepo.SetStatus(key, status);
        }


        public static event TypedEventHandler<UserContentService, SaveEventArgs<IUserContent>> Saving;
        public static event TypedEventHandler<UserContentService, SaveEventArgs<IUserContent>> Saved;

        public static event TypedEventHandler<UserContentService, DeleteEventArgs<IUserContent>> Deleting;
        public static event TypedEventHandler<UserContentService, DeleteEventArgs<IUserContent>> Deleted;

    }
}
