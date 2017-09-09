using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aubergine.Core;
using Aubergine.UserContent;
using Aubergine.UserContent.Models;
using Aubergine.UserContent.Persistance.Models;
using Semver;
using Umbraco.Core;
using Umbraco.Core.Events;

namespace Aubergine.Comments
{
    internal class Comments
    {
        public const string ProductName = "AubergineComments";
        public const string ProductVersion = "1.0.0";
        public const string Table = "Aubergine_Comments";
        public const string Instance = "Comments";
        public const string UserContentType = "comment";
    }

    public class AubergineComments : ApplicationEventHandler, IAubergineExtension
    {

        // stuff we expose to the Aubergine Dashboard
        public string Name => Comments.ProductName;
        public string ExtensionId => "{0E13068D-5972-48AE-AAC1-FAF20378B842}";
        public string Version => Comments.ProductVersion;
        public string ProductName => Comments.ProductName;

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            /// create a instance for comments, they are standard UserContentItems, but we are 
            /// storing them in their own table.
            UserContentContext.Current.LoadInstance<UserContentItem, UserContentDTO>(Comments.Instance, Comments.Table,
                applicationContext.DatabaseContext,
                applicationContext.ApplicationCache.RuntimeCache);
        }

    }
}
