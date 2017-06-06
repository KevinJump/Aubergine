using Aubergine.Core.Migrations.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;

namespace Aubergine.Complete.Migrations.TargetOneZero
{
    namespace Aubergine.Forums.Migrations.TargetOnePointZero
    {
        [Migration("1.0.0", 1, "Aubergine.Complete")]
        public class SetupUmbracoItems : MigrationBase
        {
            public SetupUmbracoItems(ISqlSyntaxProvider sqlSyntax, ILogger logger)
                : base(sqlSyntax, logger)
            {
            }

            public override void Down()
            {

            }

            public override void Up()
            {
                Logger.Info<SetupUmbracoItems>("Installing Sample Content");

                var usyncHelper = new uSyncMigrationHelper();
                usyncHelper.ImportContent("~/App_Data/Aubergine/Complete/");
            }

        }
    }
}
