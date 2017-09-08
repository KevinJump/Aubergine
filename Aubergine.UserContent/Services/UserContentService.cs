using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aubergine.UserContent.Models;
using Aubergine.UserContent.Persistance;
using Aubergine.UserContent.Persistance.Models;
using umbraco.interfaces;
using Umbraco.Core;
using Umbraco.Core.Events;

namespace Aubergine.UserContent.Services
{
    public class UserContentService<TUserContent, TUserContentDTO> : IUserContentService
        where TUserContentDTO : UserContentDTO
        where TUserContent : IUserContent
    {
        public static event TypedEventHandler<IUserContentService, SaveEventArgs<TUserContent>> Saving;
        public static event TypedEventHandler<IUserContentService, SaveEventArgs<TUserContent>> Saved;

        public static event TypedEventHandler<IUserContentService, DeleteEventArgs<TUserContent>> Deleting;
        public static event TypedEventHandler<IUserContentService, DeleteEventArgs<TUserContent>> Deleted;


        private readonly UserContentRepository<TUserContent,TUserContentDTO> _userRepo;
        private readonly ICacheRefresher _cacheRefresher;

        public UserContentService(
            UserContentRepository<TUserContent, TUserContentDTO> userContentRepository,
            ICacheRefresher cacheRefresher)
        {
            _userRepo = userContentRepository;
            _cacheRefresher = cacheRefresher;
        }

        public IUserContent Get(int id)
        {
            return _userRepo.Get(id);
        }

        public IUserContent Get(Guid key)
        {
            return _userRepo.Get(key);
        }

        public IEnumerable<IUserContent> GetApprovedUserContent(Guid contentKey)
        {
            return _userRepo.GetByContentKey(contentKey, UserContentStatus.Approved, false, string.Empty);
        }

        public IEnumerable<IUserContent> GetApprovedUserContent(Guid contentKey, string userContentType)
        {
            if (userContentType.IsNullOrWhiteSpace())
                return GetApprovedUserContent(contentKey);

            return _userRepo.GetByContentKey(contentKey, UserContentStatus.Approved, false, userContentType);
        }

        public IEnumerable<IUserContent> GetUserContent(Guid contentKey, bool getAll)
        {
            return _userRepo.GetByContentKey(contentKey, UserContentStatus.Approved, getAll, string.Empty);
        }

        public IEnumerable<IUserContent> GetUserContent(
            Guid contentKey, UserContentStatus status, 
            bool getAll, string userContentType = "")
        {
            return _userRepo.GetByContentKey(contentKey, status, getAll, userContentType);
        }

        public int GetContentCount(Guid key)
        {
            return _userRepo.GetContentCount(key);
        }

        public int GetChildCount(Guid key)
        {
            return _userRepo.GetChildCount(key);
        }

        public IEnumerable<IUserContent> GetChildren(Guid key, bool getAll = false)
        {
            return _userRepo.GetChildren(key, UserContentStatus.Approved, getAll, string.Empty);
        }

        public Attempt<IUserContent> Save(IUserContent content)
        {
            if (Saving.IsRaisedEventCancelled(new SaveEventArgs<TUserContent>((TUserContent)content), this))
                return Attempt.Fail<IUserContent>(content, new Exception("blocked by delegated event"));

            if (content.ParentKey == null)
                content.ParentKey = Guid.Empty;

            content.UpdatedDate = DateTime.Now;

            if (content.Key != null && content.Key != Guid.Empty)
            {
                var existing = _userRepo.Get(content.Key);
                if (existing != null)
                {
                    var updatedItem = _userRepo.Update(content);
                    return Attempt.Succeed<IUserContent>(updatedItem);
                }
            }

            content.Key = Guid.NewGuid();
            content.CreatedDate = DateTime.Now;

            var obj = _userRepo.Create(content);

            _cacheRefresher.RefreshAll();

            Saved.RaiseEvent(new SaveEventArgs<TUserContent>((TUserContent)obj), this);
            return Attempt.Succeed<IUserContent>(obj);
        }

        public Attempt<IUserContent> Delete(IUserContent content)
        {
            if (Deleting.IsRaisedEventCancelled(new DeleteEventArgs<TUserContent>((TUserContent)content), this))
                return Attempt.Fail<IUserContent>(content, new Exception("blocked by delegated event"));

            _userRepo.Delete(content.Key);

            Deleted.RaiseEvent(new DeleteEventArgs<TUserContent>((TUserContent)content), this);
            _cacheRefresher.Refresh(content.Key);

            return Attempt.Succeed<IUserContent>(content);
        }

        public Attempt<int> UpdateStatus(Guid key, UserContentStatus status)
        {
            var update = _userRepo.SetStatus(key, status);

            _cacheRefresher.Refresh(key);

            if (update > 0)
                return Attempt.Succeed<int>(update);
            else
                return Attempt.Fail<int>(update, new Exception("No rows updated"));
        }

        public void ClearCacheByPageKey(Guid key)
        {
            _cacheRefresher.Refresh(key);
        }
    }
}
