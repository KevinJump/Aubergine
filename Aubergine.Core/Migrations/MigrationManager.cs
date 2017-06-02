using Semver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.Migrations;

namespace Aubergine.Core.Migrations
{
    /// <summary>
    ///  manages the migrations within Aubergine - 
    ///  
    ///  instead of having this bit of code all over the shop.
    ///  each extension just needs to trigger a call to the migration
    ///  manager here to get their migration started. 
    /// </summary>
    public class MigrationManager
    {
        ApplicationContext applicationContext; 

        public MigrationManager(ApplicationContext context)
        {
            applicationContext = context;
        }

        public void ApplyMigration(string productName, SemVersion targetVersion)
        {
            var currentVersion = new SemVersion(0);

            var migrations = applicationContext.Services.MigrationEntryService.GetAll(productName);
            var latest = migrations.OrderByDescending(x => x.Version).FirstOrDefault();
            if (latest != null)
                currentVersion = latest.Version;

            if (targetVersion == currentVersion)
                return;


            var migrationRunner = new MigrationRunner(
                applicationContext.Services.MigrationEntryService,
                applicationContext.ProfilingLogger.Logger,
                currentVersion,
                targetVersion,
                productName);

            try
            {
                migrationRunner.Execute(applicationContext.DatabaseContext.Database);
            }
            catch (Exception ex)
            {
                applicationContext.ProfilingLogger.Logger.Error<MigrationManager>(string.Format("Error running {0} version: {1}", productName, targetVersion), ex);
            }

        }

    }
}
