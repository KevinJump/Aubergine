using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semver;
using Umbraco.Core;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Logging;
using Umbraco.Core.Services;

namespace Aubergine.Core.Migrations
{
    internal class MigrationManager
    {
        IMigrationEntryService _migrationService;
        ILogger _logger;
        DatabaseContext _databaseContext;

        public MigrationManager(ApplicationContext applicationContext)
        {
            _migrationService = applicationContext.Services.MigrationEntryService;
            _logger = applicationContext.ProfilingLogger.Logger;
            _databaseContext = applicationContext.DatabaseContext;
        }

        public void ApplyMigration(string productName, SemVersion targetVersion)
        {
            var currentVersion = new SemVersion(0);

            var migrations = _migrationService.GetAll(productName);
            var latest = migrations.OrderByDescending(x => x.Version).FirstOrDefault();
            if (latest != null)
                currentVersion = latest.Version;

            if (targetVersion == currentVersion)
                return;


            var migrationRunner = new MigrationRunner(
                _migrationService,
                _logger,
                currentVersion,
                targetVersion,
                productName);

            try
            {
                migrationRunner.Execute(_databaseContext.Database);
            }
            catch (Exception ex)
            {
                _logger.Error<MigrationManager>(string.Format("Error running {0} version: {1}", productName, targetVersion), ex);
            }
        }

        /// <summary>
        ///  will run migrations on anything with an IAubergineExtension
        /// </summary>
        public void ApplyExtensionMigrations()
        {
            var types = TypeFinder.FindClassesOfType<IAubergineExtension>();
            foreach (var t in types)
            {
                var instance = Activator.CreateInstance(t) as IAubergineExtension;
                if (instance != null)
                {
                    var targetVersion = SemVersion.Parse(instance.Version);
                    var productName = instance.Name;
                    ApplyMigration(productName, targetVersion);
                }
            }
        }

    }
}
