using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using Umbraco.Core.Cache;

namespace Aubergine.UserContent.Cache
{
    /// <summary>
    ///  A Null cache provider, so we can not have a cache if we don't want one.
    ///  
    ///  we return null as if the item isn't in the cache and then all the fallbacks
    ///  will have to go get things from wherever...
    /// </summary>
    public class NullCache : IRuntimeCacheProvider
    {
        public void ClearAllCache()
        {
            return;
        }

        public void ClearCacheByKeyExpression(string regexString)
        {
            return;
        }

        public void ClearCacheByKeySearch(string keyStartsWith)
        {
            return;
        }

        public void ClearCacheItem(string key)
        {
            return;
        }

        public void ClearCacheObjectTypes(string typeName)
        {
            return;
        }

        public void ClearCacheObjectTypes<T>()
        {
            return;
        }

        public void ClearCacheObjectTypes<T>(Func<string, T, bool> predicate)
        {
            return;
        }

        public object GetCacheItem(string cacheKey)
        {
            return null;
        }

        public object GetCacheItem(string cacheKey, Func<object> getCacheItem)
        {
            return null;
        }

        public object GetCacheItem(string cacheKey, Func<object> getCacheItem, TimeSpan? timeout, bool isSliding = false, CacheItemPriority priority = CacheItemPriority.Normal, CacheItemRemovedCallback removedCallback = null, string[] dependentFiles = null)
        {
            return null;
        }

        public IEnumerable<object> GetCacheItemsByKeyExpression(string regexString)
        {
            return null;
        }

        public IEnumerable<object> GetCacheItemsByKeySearch(string keyStartsWith)
        {
            return null;
        }

        public void InsertCacheItem(string cacheKey, Func<object> getCacheItem, TimeSpan? timeout = default(TimeSpan?), bool isSliding = false, CacheItemPriority priority = CacheItemPriority.Normal, CacheItemRemovedCallback removedCallback = null, string[] dependentFiles = null)
        {
            return;
        }
    }
}
