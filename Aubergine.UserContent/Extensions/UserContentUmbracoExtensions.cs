using Aubergine.UserContent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Core.Cache;
using System.Web.Caching;
using Aubergine.UserContent.Cache;
using Umbraco.Core.Models;

namespace Aubergine.UserContent
{
    public static class UserContentUmbracoExtensions
    {

        /// <summary>
        /// Get user content based on the GUID key for the content.
        /// </summary>
        /// <param name="umbraco"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IUserContent UserContent(this UmbracoHelper umbraco, Guid key, string instance = "default")
        {
            if (!UserContentContext.Current.Instances.ContainsKey(instance))
                return null;

            var _instance = UserContentContext.Current.Instances[instance];


            var itemCacheKey = $"uc_{key.ToString()}";

            var item = _instance.Cache.GetCacheItem<IUserContent>(itemCacheKey);
            if (item != null)
                return item;

            // get the item from the db. 
            item = _instance.Service.Get(key);
            if (item != null)
                _instance.Cache.InsertCacheItem<IUserContent>(itemCacheKey, () => item, priority: CacheItemPriority.Default);

            return item; 
        }

        /// <summary>
        ///  Get user list of user content associated with this umbraco node
        /// </summary>
        /// <param name="umbraco"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IEnumerable<IUserContent> UserContentByNode(this UmbracoHelper umbraco, Guid key, string instance = "default")
        {
            return GetContentByNode(key, instance);
        }

        /// <summary>
        ///  get it by type - this function needs to be better at just getting the ones we care about 
        /// </summary>
        public static IEnumerable<IUserContent> UserContentByNode(this UmbracoHelper umbraco, Guid key, string type, string instance = "default")
        {
            return GetContentByNodeAndType(key, type, instance);
        }


        public static IEnumerable<IUserContent> GetUserContent(this IPublishedContent content, string instance = "default")
        {
            return GetContentByNode(content.GetKey(), instance);
        }

        public static IEnumerable<IUserContent> GetUserContent(this IPublishedContent content, string type, string instance = "default")
        {
            return GetContentByNodeAndType(content.GetKey(), type, instance);
        }

        private static IEnumerable<IUserContent> GetContentByNode(Guid key, string instance = "default")
        {
            if (!UserContentContext.Current.Instances.ContainsKey(instance))
                return null;

            var _instance = UserContentContext.Current.Instances[instance];


            var cacheKey = $"ucp_{key.ToString()}";

            var items = new List<IUserContent>();
            var pair = _instance.Cache.GetCacheItem<UserContentParent>(cacheKey);
            if (pair != null && pair.Keys.Any())
            {
                foreach (var ikey in pair.Keys)
                {
                    var itemCacheKey = $"uc_{ikey.ToString()}";

                    items.Add(_instance.Cache.GetCacheItem<IUserContent>(itemCacheKey));
                }

                return items;
            }


            items = _instance.Service.GetByContentKey(key, false).ToList();
            if (items.Any())
            {
                // put things into the cache for next time. 
                foreach (var item in items) {
                    var itemCacheKey = $"uc_{item.Key.ToString()}";
                    _instance.Cache
                        .InsertCacheItem<IUserContent>(itemCacheKey, () => item, priority: CacheItemPriority.Default);

                }
                _instance.Cache
                    .InsertCacheItem<UserContentParent>(cacheKey, () => new UserContentParent
                    {
                        Keys = items.Select(x => x.Key).ToList<Guid>(),
                        NodeKey = key
                    }, priority: CacheItemPriority.Default);
            }

            return items;
        }

        private static IEnumerable<IUserContent> GetContentByNodeAndType(Guid key, string type, string instance = "default")
        {
            var items = GetContentByNode(key, instance);
            return items.Where(x => x.UserContentType == type);
        }

        /// <summary>
        ///  save user content back into user content storage.
        /// </summary>
        /// <param name="umbraco"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static Attempt<IUserContent> SaveUserContent(this UmbracoHelper umbraco, IUserContent item, string instance = "default")
        {
            // The service busts the cache when you save. 
            if (!UserContentContext.Current.Instances.ContainsKey(instance))
                return Attempt.Fail<IUserContent>(item, new KeyNotFoundException());

            return UserContentContext.Current.Instances[instance].Service.Save(item);
        }

        public static int GetUserContentCount(this IPublishedContent content, string userContentType, string instance = "default")
        {
            if (!UserContentContext.Current.Instances.ContainsKey(instance))
                return -1;

            var _instance = UserContentContext.Current.Instances[instance];

            var countKey = $"uc_{content.GetKey()}_count";
            var count = _instance.Cache.GetCacheItem<int?>(countKey);
            if (count != null)
                return count.Value;

            // work it out 
            var items = content.GetUserContent(userContentType, instance);
            if (items != null)
                count = items.Count();
            else
                count = 0;
            
            _instance.Cache.InsertCacheItem<int?>(countKey, () => count, priority: CacheItemPriority.Default);
            return count.Value;

        }
    }
}
