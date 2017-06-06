using Aubergine.Core;
using Semver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;

namespace Aubergine.Complete
{
    /// <summary>
    ///  Aubergine complete, just a placeholder for the full nuget package that 
    ///  will install the whole shabang in one . 
    /// </summary>
    public class AubergineComplete : ApplicationEventHandler, IAubergineExtension
    {
        SemVersion targetVersion = new SemVersion(1, 0, 0);

        public string Name => "Aubergine.Complete";
        public string ExtensionId => "{349F9962-3AE4-4C2B-8DFD-BE56D78C6793}";
        public string Version => targetVersion.ToString();

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var m = new Aubergine.Core.Migrations.MigrationManager(applicationContext);
            m.ApplyMigration("Aubergine.Complete", targetVersion);
        }
    }
}
