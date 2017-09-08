using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aubergine.UserContent.Models;
using umbraco.interfaces;
using Umbraco.Core;
using Umbraco.Core.Cache;

namespace Aubergine.UserContent.Cache
{
    public class UserContentCacheRefresher : ICacheRefresher
    {
        public Guid UniqueIdentifier => Guid.Parse("{ED99724D-477F-46CD-9728-DABAD237B741}");
        public string Name => "Aubergine.UserContent Cache Refresher";

        private IRuntimeCacheProvider _cache;

        public UserContentCacheRefresher()
        {
            _cache = ApplicationContext.Current.ApplicationCache.RuntimeCache;
        }

        public UserContentCacheRefresher(IRuntimeCacheProvider runtimeCacheProvider)
        {
            _cache = runtimeCacheProvider;
        }

        public void Refresh(int Id)
        {
            return;
        }

        public void Refresh(Guid Id)
        {
            _cache.ClearCacheByKeySearch($"uc_{Id.ToString()}");
            _cache.ClearCacheByKeySearch($"uck_{Id.ToString()}");
            _cache.ClearCacheByKeySearch($"ucpk_{Id.ToString()}");

        }

        public void RefreshAll()
        {
            _cache.ClearCacheObjectTypes<IUserContent>();
            _cache.ClearCacheObjectTypes<UserContentKeyItems>();
            _cache.ClearCacheObjectTypes<UserContentItemCount>();
        }

        public void Remove(int Id)
        {
            return;
        }
    }
}
