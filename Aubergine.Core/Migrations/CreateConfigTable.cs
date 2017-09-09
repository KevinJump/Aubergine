using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;

namespace Aubergine.Core.Migrations
{
    [Migration("1.0.0", 0, Aubergine.CoreName)]
    public class CreateConfigTable : MigrationBase
    {
        public CreateConfigTable(ISqlSyntaxProvider sqlSyntax, ILogger logger) 
            : base(sqlSyntax, logger)
        {
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            var tables = SqlSyntax.GetTablesInSchema(Context.Database).ToArray();
            if (!tables.InvariantContains("Aubergine_Configuration"))
            {
                Create.Table<AubergineConfig>();
            }

        }
    }
}
