using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aubergine.Core.Migrations.Helpers;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;

namespace Aubergine.Forums.Migrations.TargetOneZero
{
    [Migration("1.0.0", 3, AubergineForums.ProductName)]
    public class CreateTemplates : MigrationBase
    {
        public CreateTemplates(ISqlSyntaxProvider sqlSyntax, ILogger logger) 
            : base(sqlSyntax, logger)
        {
        }

        public override void Down()
        {
            // throw new NotImplementedException();
        }

        public override void Up()
        {
            var templateHelper = new TemplateManagementHelper();

            templateHelper.AddTemplate("Forums", "Forums", "design");
            templateHelper.AddTemplate("ForumPage", "ForumPage", "Forums");
            templateHelper.AddTemplate("ForumThread", "ForumThread", "Forums");
        }
    }
}
