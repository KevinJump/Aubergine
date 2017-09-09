using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aubergine.Core.Migrations;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;
using Umbraco.Core.Services;

namespace Aubergine.Auth.Migrations.TargetVersionOne
{
    [AubergineMigration(Authentication.Name, Priorities.Primary + Priorities.Template, "{70EF1630-1A4F-4B6E-AC82-9521CAE96530}")]
    public class AubergineUpdateTemplates : AubergineMigrationBase
    {
        public AubergineUpdateTemplates(ServiceContext serviceContext, ILogger logger) 
            : base(serviceContext, logger)
        {
        }

        public override void Add()
        {
            Templates.Create("Auth.Pages", TemplateAliases.Pages, "design");
            Templates.Create("Auth.Login", TemplateAliases.Login, TemplateAliases.Pages);
            Templates.Create("Auth.Reset", TemplateAliases.Reset, TemplateAliases.Pages);
            Templates.Create("Auth.Forgot", TemplateAliases.Forgot, TemplateAliases.Pages);
            Templates.Create("Auth.Verify", TemplateAliases.Verify, TemplateAliases.Pages);
            Templates.Create("Auth.Register", TemplateAliases.Register, TemplateAliases.Pages);
        }

        public override void Remove()
        {
            throw new NotImplementedException();
        }
    }
}
