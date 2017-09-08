using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aubergine.Core.Migrations.Helpers;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;

namespace Aubergine.Blog.Migrations.TargetZeroOne
{
    [Migration("1.0.0", 3, AubergineBlog.ProductName)]
    public class CreateBlogTemplates : MigrationBase
    {
        protected CreateBlogTemplates(ISqlSyntaxProvider sqlSyntax, ILogger logger) : base(sqlSyntax, logger)
        {
        }

        public override void Down()
        {
            // 
        }

        public override void Up()
        {
            var templateHelper = new TemplateManagementHelper();
            templateHelper.AddTemplate("Blog", "Blog", "design");
            templateHelper.AddTemplate("BlogCategories", "BlogCategories", "Blog");
            templateHelper.AddTemplate("BlogHome", "BlogHome", "Blog");
            templateHelper.AddTemplate("BlogPost", "BlogPost", "Blog");
            templateHelper.AddTemplate("BlogTags", "BlogTags", "Blog");
        }
    }
}
