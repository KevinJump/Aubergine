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

namespace Aubergine.Comments.Migrations
{
    [Migration("1.0.0", 1, Comments.ProductName)]
    public class CreateCommentTable : MigrationBase
    {
        public CreateCommentTable(ISqlSyntaxProvider sqlSyntax, ILogger logger) 
            : base(sqlSyntax, logger)
        {
        }

        public override void Down()
        {
            
        }

        public override void Up()
        {
            var tables = SqlSyntax.GetTablesInSchema(Context.Database).ToArray();
            if (!tables.InvariantContains(Comments.Table))
            {
                Create.Table<UserCommentDTO>();
            }
        }
    }

    [TableName(Comments.Table)]
    [PrimaryKey("id")]
    public class UserCommentDTO : Aubergine.UserContent.Persistance.Models.UserContentDTO
    { }
}
