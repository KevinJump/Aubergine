using Aubergine.UserContent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using umbraco.interfaces;
using Umbraco.Core;
using Umbraco.Core.Cache;
using Umbraco.Core.Logging;

namespace Aubergine.UserContent.Cache
{
    public class UserContentCacheRefresher : ICacheRefresher
    {
        IRuntimeCacheProvider _cache;

        public UserContentCacheRefresher() {
            _cache = ApplicationContext.Current.ApplicationCache.RuntimeCache;
        }

        public Guid UniqueIdentifier
        {
            get { return Guid.Parse("1E83BCA7-6D09-4A6E-A382-890A2003B309"); }
        }

        public string Name
        {
            get { return "UserContent Cache Refresher"; }
        }

        public void RefreshAll()
        {
            // clear the cache.
            _cache.ClearCacheObjectTypes<IUserContent>();
            _cache.ClearCacheObjectTypes<UserContentParent>();
        }

        public void Refresh(Guid id)
        {
            // clear one item from the cache. 
            _cache.ClearCacheItem(id.ToString());
        }

        public void Refresh(int Id)
        {
            return;
        }

        public void Remove(int Id)
        {
            return;
        }
    }
}
