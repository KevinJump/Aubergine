using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Cache;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;
using Umbraco.Core.Services;

namespace Aubergine.Helpers.AtoZ
{
    public class AtoZEventHandler : ApplicationEventHandler
    {
        private IRuntimeCacheProvider _runtimeCache;

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ContentService.Published += ContentService_PublishEvent;
            ContentService.UnPublished += ContentService_PublishEvent;

            _runtimeCache = applicationContext.ApplicationCache.RuntimeCache;
        }

        // when something is published or unpublished from umbraco, we clear the atoz cache. 

        private void ContentService_PublishEvent(IPublishingStrategy sender, PublishEventArgs<IContent> e)
        {
            if (_runtimeCache != null)
            {
                _runtimeCache.ClearCacheByKeySearch("atozpages");
            }
        }
    }
}
