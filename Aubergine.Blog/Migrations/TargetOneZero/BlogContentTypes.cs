using Aubergine.Core.Migrations.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;
using Umbraco.Core.Services;

namespace Aubergine.Blog.Migrations.TargetOneZero
{
    [Migration("1.0.0", 1, "Blogs")]
    public class BlogContentTypes : MigrationBase
    {
        public BlogContentTypes(ISqlSyntaxProvider sqlSyntax, ILogger logger) 
            : base(sqlSyntax, logger)
        {
        }

        public override void Down()
        {
        }

        public override void Up()
        {
            // nuget / the zip and the package will dump things in app_data/aubergine/blog...
            var usyncHelper = new uSyncMigrationHelper();
            usyncHelper.Import("~/App_Data/Aubergine/Blog/");
        }

    }
}
