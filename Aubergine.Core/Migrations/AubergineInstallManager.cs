using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Logging;
using Umbraco.Core.Configuration;

namespace Aubergine.Core.Migrations
{
    /// <summary>
    ///  will run all unran AubergineMigrations
    ///  
    ///  Aubergine Migrations are not like UmbracoMigrations
    ///  they don't form part of a 'product' they are all
    ///  unique 
    /// </summary>
    public class AubergineInstallManager
    {
        private ILogger _logger;
        private DatabaseContext _dbContext;
        private AubergineMigrationService _configService;

        public AubergineInstallManager(
            DatabaseContext databaseContext,
            ILogger logger)
        {
            _dbContext = databaseContext;
            _logger = logger;

            _configService = new AubergineMigrationService(_dbContext);
        }

        public bool Execute()
        {
            var configurations = AubergineMigrationResolver.Current.Configurations;
            var pendingConfigs = GetRequiredConfigurations(configurations);

            // now run any of the migrations that haven't already been ran.
            foreach (var item in pendingConfigs)
            {
                try
                {
                    _logger.Info<AubergineInstallManager>("Running Config: {0} {1}",
                        () => item.Name, () => item.SortOrder);

                     item.Configuration.Add();
                    _configService.Add(item.Name, item.Key);
                }
                catch (Exception ex)
                {
                    _logger.Warn<AubergineInstallManager>("Migration Failed: {0} {1}",
                        () => item.Configuration.GetType().Name, () => ex.Message);
                }
            }

            // we fire a raise event, this is what something can safely use
            // to know all the aubergine bits are in place...
            Migrated.RaiseEvent(new EventArgs(), this);

            return true;
        }

        private IEnumerable<AubergineConfigInfo> GetRequiredConfigurations(IEnumerable<IAubergineMigration> configurations)
        {
            var installed = _configService.GetAll();

            var configs = new List<AubergineConfigInfo>();

            foreach (var configuration in configurations)
            {

                var attribs = configuration.GetType().GetCustomAttribute<AubergineMigrationAttribute>(false);
                if (attribs != null)
                {
                    if (!installed.Any(x => x.Key == attribs.Key))
                    {
                        var config = new AubergineConfigInfo
                        {
                            Name = attribs.ItemName,
                            Key = attribs.Key,
                            SortOrder = attribs.SortOrder,
                            Configuration = configuration
                        };


                        configs.Add(config);
                    }
                }
            }

            return configs.OrderBy(x => x.SortOrder);
        }

        internal class AubergineConfigInfo
        {
            public string Name { get; set; }
            public string Key { get; set; }
            public int SortOrder { get; set; }
            public IAubergineMigration Configuration { get; set; }
        }

        // called once everything is ran
        public static event TypedEventHandler<AubergineInstallManager, EventArgs> Migrated;
    }

}
