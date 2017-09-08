using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using Umbraco.Core.Cache;
using Aubergine.UserContent.Models;

namespace Aubergine.UserContent
{
    /// <summary>
    ///  IUserContent Extensions for getting Cached IUserContent based on an existing bit of UserContent 
    ///  (so children etc)
    /// </summary>
    public static class UserContentExtensions
    {
        public static IEnumerable<IUserContent> Children(this IUserContent content, string instance = "default")
        {
            if (!UserContentContext.Current.Instances.ContainsKey(instance))
                return null;

            return UserContentContext.Current.Instances[instance].Service.GetChildren(content.Key, false);
        }

        public static IUserContent Parent(this IUserContent content, string instance = "default")
        {
            if (content.ParentKey == Guid.Empty)
                return null;

            if (!UserContentContext.Current.Instances.ContainsKey(instance))
                return null;

            var _instance = UserContentContext.Current.Instances[instance];

            var parent = _instance.Cache.GetCacheItem<IUserContent>(content.ParentKey.ToString());
            if (parent != null)
                return parent;

            parent = _instance.Service.Get(content.ParentKey);
            if (parent != null)
                _instance.Cache.InsertCacheItem<IUserContent>
                    (content.Key.ToString(), () => content, priority: CacheItemPriority.Default);

            return parent;
        }

        public static string GetPropertyValue(this IUserContent content, string alias, string defaultValue)
        {
            var item = content.GetProperty(alias);
            return item != null ? item.Value.ToString() : defaultValue;
        }

        public static object GetPropertyValue(this IUserContent content, string alias, object defaultValue)
        {
            var item = content.GetProperty(alias);
            return item != null ? item.Value : defaultValue;
        }

        public static TValue GetPropertyValue<TValue>(this IUserContent content, string alias, TValue defaultValue)
        {
            var item = content.GetProperty(alias);
            return item != null ? (TValue)item.Value : defaultValue;
        }

        public static void SetProperty(this IUserContent content, string alias, object value)
        {
            if (!content.Properties.Any(x => x.PropertyAlias == alias))
            {
                content.Properties.Add(new UserContentProperty
                {
                    PropertyAlias = alias,
                    Value = value
                });
            }
            else
            {
                var existing = content.Properties.FirstOrDefault(x => x.PropertyAlias == alias);
                if (existing != null)
                    existing.Value = value;
            }
        }
        public static void SetProperty(this IUserContent content, string alias, string value)
        {
            if (!content.Properties.Any(x => x.PropertyAlias == alias))
            {
                content.Properties.Add(new UserContentProperty
                {
                    PropertyAlias = alias,
                    Value = value
                });
            }
            else
            {
                var existing = content.Properties.FirstOrDefault(x => x.PropertyAlias == alias);
                if (existing != null)
                    existing.Value = value;
            }
        }

        public static void SetProperty<TValue>(this IUserContent content, string alias, TValue value)
        {
            if (!content.Properties.Any(x => x.PropertyAlias == alias))
            {
                content.Properties.Add(new UserContentProperty
                {
                    PropertyAlias = alias,
                    Value = value
                });
            }
            else
            {
                var existing = content.Properties.FirstOrDefault(x => x.PropertyAlias == alias);
                if (existing != null)
                    existing.Value = value;
            }
        }

        public static void AddOrUpdateProperty<TValue>(this IList<UserContentProperty> Properties, string alias, TValue value)
        {
            var property = Properties.FirstOrDefault(x => x.PropertyAlias == alias);
            if (property != null)
            {
                property.Value = value;
            }
            else
            {
                Properties.Add(new UserContentProperty(alias, value));
            }
        }
    }
}
