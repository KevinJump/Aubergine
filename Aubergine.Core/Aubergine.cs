using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aubergine.Core.Migrations;
using Umbraco.Core;

namespace Aubergine.Core
{
    public class Aubergine : ApplicationEventHandler
    {
        protected override void ApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var serviceProvider = new AubergineServiceProvider(applicationContext);
            AubergineMigrationResolver.Current = new AubergineMigrationResolver
                (serviceProvider, applicationContext.ProfilingLogger.Logger,
                () => PluginManager.Current.ResolveTypes<IAubergineMigration>());
        }

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            // run any umbraco Migrations in any IAubergineExtension assemblies
            var umbracoMigrations = new MigrationManager(applicationContext);
            umbracoMigrations.MigrateAubergineExtensions();

            // run all the aubergine configurations
            var aubergineMigrator = new AubergineInstallManager(
                applicationContext.DatabaseContext,
                applicationContext.ProfilingLogger.Logger);
            aubergineMigrator.Execute();
        }
    }
}
