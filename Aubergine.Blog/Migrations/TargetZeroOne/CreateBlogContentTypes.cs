using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aubergine.Core.Migrations.Helpers;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;

namespace Aubergine.Blog.Migrations.TargetZeroOne
{
    [Migration("1.0.0", 4, AubergineBlog.ProductName)]
    class CreateBlogContentTypes : MigrationBase
    {
        public CreateBlogContentTypes(ISqlSyntaxProvider sqlSyntax, ILogger logger) 
            : base(sqlSyntax, logger)
        {
        }

        public override void Down()
        {
            // 
        }

        public override void Up()
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;
            var contentTypeService = ApplicationContext.Current.Services.ContentTypeService;

            var contentTypeHelper = new ContentTypeManagementHelper(
                contentTypeService, dataTypeService);

            var folderId = contentTypeHelper.CreateFolder("Aub.Blog");
            contentTypeHelper.CreateFromFolder("~/App_Data/Aubergine/Blog/ContentTypes/", folderId);
        }
    }
}
