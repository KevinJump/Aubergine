using Aubergine.UserContent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using Umbraco.Core;
using Umbraco.Core.Cache;

namespace Aubergine.UserContent
{
    public static class UserContentExtensions
    {
        /// <summary>
        ///  gets the children of this item (misses the cache)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static IEnumerable<IUserContent> Children(this IUserContent item, string instance = "default")
        {
            if (!UserContentContext.Current.Instances.ContainsKey(instance))
                return null;

            return UserContentContext.Current.Instances[instance].Service.GetChildren(item.Key, false);
        }

        /// <summary>
        ///  gets the parent of this item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static IUserContent Parent(this IUserContent item, string instance = "default")
        {
            if (!UserContentContext.Current.Instances.ContainsKey(instance))
                return null;

            var _instance = UserContentContext.Current.Instances[instance];


            if (item.ParentKey != Guid.Empty)
            {
                var parent = _instance.Cache.GetCacheItem<IUserContent>(item.ParentKey.ToString());
                if (parent != null)
                    return parent;

                // get the item from the db. 
                parent = _instance.Service.Get(item.ParentKey);
                if (parent != null)
                    _instance.Cache.InsertCacheItem<IUserContent>(item.Key.ToString(), () => item, priority: CacheItemPriority.Default);

                return parent;

            }

            return null;

        }

        public static void SetProperty(this IUserContent item, string alias, object value)
        {
            if (!item.Properties.Any(x => x.PropertyAlias == alias))
            {
                item.Properties.Add(new UserContentProperty
                {
                    PropertyAlias = alias,
                    Value = value
                });
            }
            else
            {
                var existing = item.Properties.FirstOrDefault(x => x.PropertyAlias == alias);
                if (existing != null)
                    existing.Value = value;
            }
        }

        public static void SetProperty(this IUserContent item, string alias, string value)
        {
            if (!item.Properties.Any(x => x.PropertyAlias == alias))
            {
                item.Properties.Add(new UserContentProperty
                {
                    PropertyAlias = alias,
                    Value = value
                });
            }
            else
            {
                var existing = item.Properties.FirstOrDefault(x => x.PropertyAlias == alias);
                if (existing != null)
                    existing.Value = value;
            }
        }

        public static void SetProperty<T>(this IUserContent item, string alias, T value)
        {
            if (!item.Properties.Any(x => x.PropertyAlias == alias))
            {
                item.Properties.Add(new UserContentProperty
                {
                    PropertyAlias = alias,
                    Value = value
                });
            }
            else
            {
                var existing = item.Properties.FirstOrDefault(x => x.PropertyAlias == alias);
                if (existing != null)
                    existing.Value = value;
            }
        }

        public static object GetPropertyValue(this IUserContent content, string alias, string defaultValue)
        {
            var item = content.GetProperty(alias);
            return item != null ? item.Value : defaultValue;
        }

        public static object GetPropertyValue(this IUserContent content, string alias, object defaultValue)
        {
            var item = content.GetProperty(alias);
            return item != null ? item.Value : defaultValue;
        }

        public static T GetPropertyValue<T>(this IUserContent content, string alias, T defaultValue)
        {
            var item = content.GetProperty(alias);
            return item != null ? (T)item.Value : defaultValue;
        }
    }
}
