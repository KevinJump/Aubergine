using Aubergine.Core;
using Aubergine.UserContent.Persistance;
using Aubergine.UserContent.Persistance.Mappers;
using AutoMapper;
using Semver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;

namespace Aubergine.UserContent
{
    public class AubergineUserContent : ApplicationEventHandler, IAubergineExtension
    {
        private readonly SemVersion targetVersion = new SemVersion(1, 0, 0);

        public string Name => "UserContent";
        public string ExtensionId => "{BFAE4E4B-864F-474F-9AC6-6F910310C2D0}";
        public string Version => targetVersion.ToString();

        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            Mapper.CreateMap<UserContentDTO, UserContentItem>()
                .ForMember(x => x.Properties,
                    opt => opt.ResolveUsing(new UserContentPropertiesResolver()));

            Mapper.CreateMap<UserContentItem, UserContentDTO>()
                .ForMember(x => x.PropertyData,
                    opt => opt.ResolveUsing(new UserContentToDTOPropertiesResolver()));

            var ctx = UserContentContext.EnsureContext(applicationContext.ProfilingLogger.Logger);

            // load the default instance (pointing to umbraco, and an umbraco table)
            ctx.LoadInstance("default", "UserContent",
                applicationContext.DatabaseContext,
                applicationContext.ApplicationCache.RuntimeCache);

            // you can now load your own instances into the context, and use them to store your things in other places.
            // 

        }

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var migrationManager = new Aubergine.Core.Migrations.MigrationManager(applicationContext);
            migrationManager.ApplyMigration(Name, targetVersion);
        }

    }
}
