using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aubergine.Forums.Models;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;

namespace Aubergine.Forums.Migrations
{
    [Migration("1.0.0", 1, AubergineForums.ProductName)]
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
            var tables = SqlSyntax.GetTablesInSchema(Context.Database);
            if (!tables.InvariantContains(AubergineForums.Table))
                Create.Table<ForumPostsDTO>();
        }
    }
}
