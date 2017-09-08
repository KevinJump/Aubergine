using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aubergine.UserContent.Models;
using Aubergine.UserContent.Persistance.Mappers;
using Aubergine.UserContent.Persistance.Models;
using Semver;
using Umbraco.Core;

namespace Aubergine.UserContent
{
    public class UserContentApplicationEvents : ApplicationEventHandler
    {
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {

            // initizlie the user content context. 
            UserContentContext.EnsureContext(
                applicationContext.ProfilingLogger.Logger,
                applicationContext.ApplicationCache.RuntimeCache
                );

            // We add the default context. 
            // but you can add your own with your own tables 
            // in your own projects. 

            // however it is your responsiblity for the tables to be 
            UserContentContext.Current.LoadInstance<UserContentItem,UserContentDTO>(
                UserContent.DefaultInstance, "Aubergine_UserContent",
                applicationContext.DatabaseContext,
                applicationContext.ApplicationCache.RuntimeCache);
        }

        protected override void ApplicationStarted(
            UmbracoApplicationBase umbracoApplication, 
            ApplicationContext applicationContext)
        {
            var migrationHelper = new Aubergine.Core.MigrationManager(applicationContext);
            migrationHelper.ApplyMigration(UserContent.Name, SemVersion.Parse(UserContent.Version));

            var mappings = new InitializeMappers();
            mappings.CreateMappings();
        }
    }
}
