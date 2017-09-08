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
    public class Comments : ApplicationEventHandler, IAubergineExtension
    {
        public const string ProductName = "AubergineComments";
        public const string ProductVersion = "1.0.0";
        public const string Table = "Aubergine_Comments";
        public const string Instance = "Comments";
        public const string UserContentType = "comment";

        // stuff we expose to the Aubergine Dashboard
        public string Name => ProductName;
        public string ExtensionId => "{0E13068D-5972-48AE-AAC1-FAF20378B842}";
        public string Version => ProductVersion;

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var migrationHelper = new Aubergine.Core.MigrationManager(applicationContext);
            migrationHelper.ApplyMigration(ProductName, SemVersion.Parse(ProductVersion));

            /// create a instance for comments, they are standard UserContentItems, but we are 
            /// storing them in their own table.
            UserContentContext.Current.LoadInstance<UserContentItem, UserContentDTO>(Instance, Table,
                applicationContext.DatabaseContext,
                applicationContext.ApplicationCache.RuntimeCache);
        }

    }
}
