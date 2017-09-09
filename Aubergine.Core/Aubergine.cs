using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aubergine.Core.Migrations;
using Semver;
using Umbraco.Core;

namespace Aubergine.Core
{
    public class Aubergine : ApplicationEventHandler
    {
        public const string CoreVersion = "1.0.0";
        public const string CoreName = "Aubergine.Core";

        protected override void ApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var serviceProvider = new AubergineServiceProvider(applicationContext);
            AubergineMigrationResolver.Current = new AubergineMigrationResolver
                (serviceProvider, applicationContext.ProfilingLogger.Logger,
                () => PluginManager.Current.ResolveTypes<IAubergineMigration>());
        }

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var umbracoMigrations = new MigrationManager(applicationContext);

            // create the config table
            umbracoMigrations.ApplyMigration(CoreName, SemVersion.Parse(CoreVersion));

            // run any Aubergine Migrations in any IAubergineExtension assemblies
            umbracoMigrations.ApplyExtensionMigrations();

            // run all the aubergine configurations
            var aubergineMigrator = new AubergineInstallManager(
                applicationContext.DatabaseContext,
                applicationContext.ProfilingLogger.Logger);
            aubergineMigrator.Execute();
        }
    }
}
