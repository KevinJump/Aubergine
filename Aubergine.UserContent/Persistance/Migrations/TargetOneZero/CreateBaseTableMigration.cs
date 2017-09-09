using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aubergine.UserContent.Persistance.Models;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;

namespace Aubergine.UserContent.Persistance.Migrations.TargetOneZero
{
    [Migration("1.0.0", 1, UserContent.Name)]
    public class CreateBaseTableMigration : MigrationBase
    {
        public CreateBaseTableMigration(ISqlSyntaxProvider sqlSyntax, ILogger logger) 
            : base(sqlSyntax, logger)
        {
        }

        public override void Down()
        {
            // 
        }

        public override void Up()
        {
            var tables = SqlSyntax.GetTablesInSchema(Context.Database).ToArray();
            if (!tables.InvariantContains(UserContent.TableName))
            {
                Create.Table<UserContentDTO>();

                /*
                Create.Index("IX_UserContentKeyIndex")
                    .OnTable(UserContent.TableName)
                    .OnColumn("Key")
                    .Unique().WithOptions().NonClustered(); */

            }
        }
    }
}
