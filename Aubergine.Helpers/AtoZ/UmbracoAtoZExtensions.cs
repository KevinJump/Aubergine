using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Cache;
using Umbraco.Core.Models;
using Umbraco.Web;
using System.Web.Caching;
using System.Diagnostics;
using Umbraco.Core.Logging;

namespace Aubergine.Helpers
{
    public static class UmbracoAtoZExtensions
    {
        public static List<AtoZInfo> GetAtoZEntries(this UmbracoHelper umbraco, string letter)
        {
            return GetAtoZEntries(umbraco, letter, new string[] { });
        }

        public static List<AtoZInfo> GetAtoZEntries(this UmbracoHelper umbraco, string letter, string[] excludedTypes)
        {
            var root = umbraco.TypedContentAtRoot().First();
            return GetAtoZEntries(umbraco, letter, excludedTypes, root);
        }

        public static List<AtoZInfo> GetAtoZEntries(this UmbracoHelper umbraco, string letter, string[] excludedTypes, IPublishedContent atozRoot)
        {
            var entries = GetAtoZPages(umbraco, excludedTypes, atozRoot);
            var l = letter.ToLower();

            return entries.Where(x => x.Key.StartsWith(l))
                .Select(x => x.Value)
                .ToList();

        }

        private static SortedDictionary<string, AtoZInfo> GetAtoZPages(UmbracoHelper umbraco, string[] excludedTpes, IPublishedContent root)
        {
            var cacheName = $"atozpages_{root.Id}";
            var cache = ApplicationContext.Current.ApplicationCache.RuntimeCache;

            var atozPages = cache.GetCacheItem<SortedDictionary<string, AtoZInfo>>(cacheName);

            if (atozPages == null)
            {
                // no cache - so we need to go create it. 
                Stopwatch sw = Stopwatch.StartNew();

                atozPages = new SortedDictionary<string, AtoZInfo>();

                // not an ideal query, but we are caching it - 
                // if you where clever with xpath you might do this better
                var sitePages = root.Descendants().Where(
                                    x => x.IsVisible()
                                    && !excludedTpes.InvariantContains(x.DocumentTypeAlias)
                                    && !x.GetPropertyValue<bool>("excludeFromAtoZ")
                                    && !x.HasProperty("isComponent"));

                foreach(var page in sitePages)
                {
                    var title = page.GetPropertyValue<string>("title", page.Name).Trim();
                    if (!atozPages.ContainsKey(title.ToLower()))
                    {
                        atozPages.Add(title.ToLower(), new AtoZInfo
                        {
                            Title = title,
                            Id = page.Id,
                            Url = page.Url
                        });
                    }
                }

                cache.InsertCacheItem<SortedDictionary<string, AtoZInfo>>
                    (cacheName, () => atozPages, priority: CacheItemPriority.Default);

                sw.Stop();
                LogHelper.Debug<AtoZInfo>("AtoZ build [{0}] {1} pages {2}ms",
                    () => root.Id, () => atozPages.Count, () => sw.ElapsedMilliseconds);
                    
            }

            return atozPages;
        }
    }
}
