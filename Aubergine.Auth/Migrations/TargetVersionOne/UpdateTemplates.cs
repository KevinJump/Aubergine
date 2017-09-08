using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;

namespace Aubergine.Auth.Migrations.TargetVersionOne
{
    [Migration("1.0.0", 0, Product.Name)]
    public class UpdateTemplates : MigrationBase
    {
        public UpdateTemplates(ISqlSyntaxProvider sqlSyntax, ILogger logger)
            : base(sqlSyntax, logger) { }

        public override void Up()
        {
            var templateHelper = new Aubergine.Core.Migrations.Helpers.TemplateManagementHelper();

            templateHelper.AddTemplate("Auth.Pages", TemplateAliases.Pages, "design");
            templateHelper.AddTemplate("Auth.Login", TemplateAliases.Login, TemplateAliases.Pages);
            templateHelper.AddTemplate("Auth.Reset", TemplateAliases.Reset, TemplateAliases.Pages);
            templateHelper.AddTemplate("Auth.Forgot", TemplateAliases.Forgot, TemplateAliases.Pages);
            templateHelper.AddTemplate("Auth.Verify", TemplateAliases.Verify, TemplateAliases.Pages);
            templateHelper.AddTemplate("Auth.Register", TemplateAliases.Register, TemplateAliases.Pages);
        }

        public override void Down()
        { }
    }


}
