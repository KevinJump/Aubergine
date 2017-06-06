using Aubergine.Core.Migrations.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;

namespace Aubergine.Forums.Migrations.TargetOnePointZero
{
    [Migration("1.0.0", 1, Product.Name)]
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
            var usyncHelper = new uSyncMigrationHelper();
            usyncHelper.Import("~/App_Data/Aubergine/Forums/");
        }

    }
}
