using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aubergine.Core.Migrations;
using Aubergine.Core.Migrations.Helpers;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;
using Umbraco.Core.Services;

namespace Aubergine.Forums.Migrations.TargetOneZero
{
    [AubergineMigrationAttribute(Forums.ProductName, Priorities.Standard + Priorities.Template, 
        "{055D32EA-FA62-4566-8CBB-5CAC163ECFBB}")]
    public class AubergineCreateTemplates : AubergineMigrationBase
    {
        public AubergineCreateTemplates(ServiceContext serviceContext, ILogger logger) 
            : base(serviceContext, logger)
        {
        }

        public override void Add()
        {
            Templates.Create("Forums", "Forums", "design");
            Templates.Create("ForumPage", "ForumPage", "Forums");
            Templates.Create("ForumThread", "ForumThread", "Forums");
        }

        public override void Remove()
        {
            throw new NotImplementedException();
        }
    }

}
