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


namespace Aubergine.Blog.Migrations.TargetZeroOne
{
    [AubergineMigration("Create Blog Templates", 
        Priorities.Standard + Priorities.Template
        , "{9E2B3E18-909B-4637-B9D4-618A9D971284}")]
    public class AubergineCreateBlogTemplates : AubergineMigrationBase
    {
        public AubergineCreateBlogTemplates(ServiceContext serviceContext, ILogger logger) 
            : base(serviceContext, logger)
        {
        }

        public override void Add()
        {
            Templates.Create("Blog", "Blog", "design");
            Templates.Create("BlogCategories", "BlogCategories", "Blog");
            Templates.Create("BlogHome", "BlogHome", "Blog");
            Templates.Create("BlogPost", "BlogPost", "Blog");
            Templates.Create("BlogTags", "BlogTags", "Blog");
        }

        public override void Remove()
        {
            throw new NotImplementedException();
        }
    }
}
