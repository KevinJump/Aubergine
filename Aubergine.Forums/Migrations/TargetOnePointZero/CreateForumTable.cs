using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;

namespace Aubergine.Forums.Migrations.TargetOnePointZero
{
    [Migration("1.0.0", 1, Product.Name)]
    public class CreateForumTable : MigrationBase
    {
        public CreateForumTable(ISqlSyntaxProvider sqlSyntax, ILogger logger)
            : base(sqlSyntax, logger)
        {
        }

        public override void Down()
        {

        }

        public override void Up()
        {
            var tables = SqlSyntax.GetTablesInSchema(Context.Database).ToArray();
            if (!tables.InvariantContains(Product.Table))
            {
                Create.Table<UserForumDTO>();
            }
        }
    }

    // a punt - we want to create our own comment table, so comments are 
    // stored in a diffrent location, the user content service/repo support
    // this, but you need to create the table. 
    // rather than putting in stupid amounds of sql - lets try just inheriting
    // the base table and changing properties. 
    [TableName(Product.Table)]
    [PrimaryKey("id")]
    public class UserForumDTO : Aubergine.UserContent.Persistance.UserContentDTO
    {

    }
}