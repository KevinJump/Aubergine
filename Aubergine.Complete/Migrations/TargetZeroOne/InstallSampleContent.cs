using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;

namespace Aubergine.Complete.Migrations.TargetZeroOne
{
    [Migration("1.0.0", 1, AubergineComplete.ProductName)]
    public class InstallSampleContent : MigrationBase
    {
        public InstallSampleContent(ISqlSyntaxProvider sqlSyntax, ILogger logger) 
            : base(sqlSyntax, logger)
        {
        }

        public override void Down()
        {

        }

        public override void Up()
        {
            // import the sample content? 
        }
    }
}
