using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Core.Logging;

namespace Aubergine
{
    /// <summary>
    ///  a settings handler. will load (and refresh as needed) a link
    ///  to the siteSettings node, this means you can just call
    ///  
    ///  Aubergine.SiteSettings<string>("someProperty","default"); 
    ///  
    ///  To read a setting. 
    ///  
    ///  Within your own classes you can also subscribe to the SettingsChanged
    ///  event, so when settings change - you can get them.
    /// </summary>
    public partial class Aubergine
    {
        public static IPublishedContent GlobalSettings { get; set; }

        public static T SiteSettings<T>(string key)
        {
            return SiteSettings<T>(key, default(T));
        }

        public static T SiteSettings<T>(string key, T defaultValue)
        {
            if (GlobalSettings != null 
                && GlobalSettings.HasProperty(key)
                && GlobalSettings.HasValue(key))
            {
                return GlobalSettings.GetPropertyValue<T>(key, defaultValue);
            }
            return defaultValue;
        }

        public static event GlobalSettingsEventHandler SettingsChanged;
        public delegate void GlobalSettingsEventHandler(EventArgs e);

        internal static void LoadSettingsNode(bool raiseEvents = true)
        {
            UmbracoHelper umbraco = new UmbracoHelper(UmbracoContext.Current);
            var settings = umbraco.TypedContentSingleAtXPath("//siteSettings");
            if (settings != null)
            {
                Aubergine.GlobalSettings = settings;

                if (raiseEvents && SettingsChanged != null)
                    SettingsChanged(new EventArgs());
            }
        }
    }

    public class SiteSettingEventHandler : ApplicationEventHandler
    {
        int settingsContentTypeId = -1;
        ILogger _logger; 

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            _logger = applicationContext.ProfilingLogger.Logger;

            Aubergine.LoadSettingsNode(false);


            ContentService.Published += ContentService_Published;

            var contentType = 
                applicationContext.Services.ContentTypeService.GetContentType("siteSettings");
            if (contentType != null)
                settingsContentTypeId = contentType.Id;

        }

        private void ContentService_Published(Umbraco.Core.Publishing.IPublishingStrategy sender, Umbraco.Core.Events.PublishEventArgs<IContent> e)
        {
            try
            {
                if (settingsContentTypeId != -1)
                {
                    foreach(var item in e.PublishedEntities)
                    {
                        if (item.ContentTypeId == settingsContentTypeId)
                        {
                            Aubergine.LoadSettingsNode();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.Warn<SiteSettingEventHandler>("Failed to re-load settings: {0}", ()=> ex.ToString());
            }
        }
    }
}
