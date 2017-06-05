using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Persistence.Migrations;
using Aubergine.Comments;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.SqlSyntax;
using Umbraco.Core.Persistence;

namespace Aubergine.Comments.Migrations.TargetOnePointZero
{
    [Migration("1.0.0", 1, Product.Name)]
    public class CreateCommentsUserContentTable : MigrationBase
    {
        public CreateCommentsUserContentTable(ISqlSyntaxProvider sqlSyntax, ILogger logger) 
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
                Create.Table<UserCommentDTO>();
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
    public class UserCommentDTO : Aubergine.UserContent.Persistance.UserContentDTO
    {

    }
}
