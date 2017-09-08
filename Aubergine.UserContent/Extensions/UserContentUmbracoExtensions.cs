using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aubergine.UserContent.Models;
using Umbraco.Web;
using Umbraco.Core.Cache;
using System.Web.Caching;
using Aubergine.UserContent.Cache;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace Aubergine.UserContent
{
    /// <summary>
    ///  Extensions to the UmbracoHelper for getting/saving Cached IUserContent
    /// </summary>
    public static class UserContentUmbracoExtensions
    {
        // cache prefixes.

        // individual nodes
        public const string UserContentPrefix = "uc";

        // key pairs for children of nodes
        public const string UserContentKeysPrefix = "uck";

        // key pairs for chidlren of a IPublishedContent item
        public const string IPublishedKeysPrefix = "ucpk";

        internal static IUserContent GetCachedUserContent(Guid key, 
            string cachePrefix, string instance)
        {
            if (!UserContentContext.Current.Instances.ContainsKey(instance))
                return null;

            var _instance = UserContentContext.Current.Instances[instance];

            var cacheKey = $"{cachePrefix}_{key.ToString()}";
            var item = _instance.Cache.GetCacheItem<IUserContent>(cacheKey);
            if (item == null)
            {
                item = _instance.Service.Get(key);
                if (item != null)
                    _instance.Cache.InsertCacheItem<IUserContent>
                        (cacheKey, () => item, priority: CacheItemPriority.Default);
            }
            return item;
        }

        internal static IEnumerable<IUserContent> GetCachedUserContentByNode(Guid key, 
            string cachePrefix,
            string userContentType,
            string instance)
        {
            if (!UserContentContext.Current.Instances.ContainsKey(instance))
                return null;

            var _instance = UserContentContext.Current.Instances[instance];
            var cacheKey = $"{cachePrefix}_{key.ToString()}";

            if (userContentType != "")
                cacheKey += $"_{userContentType}";

            var items = new List<IUserContent>();
            var pairs = _instance.Cache.GetCacheItemsByKeySearch<UserContentKeyItems>(cacheKey);
            if (pairs != null && pairs.Any())
            {
                foreach (var pair in pairs)
                {
                    foreach (var itemKey in pair.ItemKeys)
                    {
                        var item = GetCachedUserContent(itemKey, UserContentPrefix, instance);
                        if (item != null)
                            items.Add(item);
                    }
                }

                return items;
            }

            // else get it from the db
            items = _instance.Service.GetApprovedUserContent(key)
                .ToList();

            if (items.Any())
            {
                // put what we have found into the cache. 
                foreach (var item in items)
                {
                    var itemCacheKey = $"{UserContentPrefix}_{key.ToString()}";
                    if (!item.UserContentType.IsNullOrWhiteSpace())
                        itemCacheKey += $"_{item.UserContentType}";

                    _instance.Cache.InsertCacheItem<IUserContent>
                        (itemCacheKey, () => item, priority: CacheItemPriority.Default);
                }

                _instance.Cache.InsertCacheItem<UserContentKeyItems>
                    (cacheKey, () => new UserContentKeyItems
                    {
                        Key = key,
                        ItemKeys = items.Select(x => x.Key).ToList<Guid>()
                    }, priority: CacheItemPriority.Default);
            }

            return items;

        }


        /// <summary>
        ///  Get any IUserContent associated with the IPublishedContent node with this id
        /// </summary>
        [Obsolete("Getting UserContent by Guid is recommented", false)]
        public static IEnumerable<IUserContent> GetUserContent(this UmbracoHelper umbraco, int id, 
            string userContentType = "",
            string instance = Aubergine.UserContent.UserContent.DefaultInstance)
        {
            var content = umbraco.TypedContent(id);
            return GetCachedUserContentByNode(content.GetKey(), IPublishedKeysPrefix, userContentType, instance);
        }

        /// <summary>
        ///  Get any IUserContent accociated with the IPublishedContent node with this Key
        /// </summary>
        public static IEnumerable<IUserContent> GetUserContent(this UmbracoHelper umbraco, Guid key,
            string userContentType = "",
            string instance = Aubergine.UserContent.UserContent.DefaultInstance)
        {
            return GetCachedUserContentByNode(key, IPublishedKeysPrefix, userContentType, instance);
        }

        /// <summary>
        ///  Get any IUserContent associatied with this IPublishedContent page. 
        /// </summary>
        public static IEnumerable<IUserContent> GetUserContent(this IPublishedContent content, 
            string userContentType = "",
            string instance = Aubergine.UserContent.UserContent.DefaultInstance)
        {
            return GetCachedUserContentByNode(content.GetKey(), IPublishedKeysPrefix, userContentType, instance);
        }

        /// <summary>
        ///  get a single piece of user content by it's ID
        /// </summary>
        public static IUserContent UserContent(this UmbracoHelper umbraco, Guid key, string instance = "default")
        {
            return GetCachedUserContent(key, UserContentKeysPrefix, instance);
        }

        /// <summary>
        ///  Save a bit of UserContent back to the db.
        /// </summary>
        public static Attempt<IUserContent> SaveUserContent(this UmbracoHelper umbraco, 
        IUserContent content, string instance = "default")
        {
            if (!UserContentContext.Current.Instances.ContainsKey(instance))
                return Attempt.Fail<IUserContent>(content, new KeyNotFoundException());

            return UserContentContext.Current.Instances[instance].Service.Save(content);
        }

        public static int GetUserContentCount(this IPublishedContent content, 
            string userContentType, string instance = "default")
        {
            if (!UserContentContext.Current.Instances.ContainsKey(instance))
                return -1;

            var _instance = UserContentContext.Current.Instances[instance];

            var countKey = $"uc_{content.GetKey()}_count";
            var count = _instance.Cache.GetCacheItem<UserContentItemCount>(countKey);
            if (count != null)
                return count.Count;

            // not in cache go get it.
            var dbCount = _instance.Service.GetContentCount(content.GetKey());
            if (dbCount > 0)
            {
                _instance.Cache.InsertCacheItem<UserContentItemCount>
                    (countKey, () => new UserContentItemCount
                    {
                        Count = dbCount
                    }, priority: CacheItemPriority.Default);
            }
            return dbCount;
        }
    }
}
