using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;

namespace Aubergine.Forums.Migrations
{
    [Migration("1.0.0", 3, AubergineForums.ProductName)]
    public class CreateContentType : MigrationBase
    {
        public CreateContentType(ISqlSyntaxProvider sqlSyntax, ILogger logger) : base(sqlSyntax, logger)
        {
        }

        public override void Down()
        {
            
        }

        public override void Up()
        {
            var contentTypeService = ApplicationContext.Current.Services.ContentTypeService;
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;

            var contentTypeHelper = new Aubergine.Core.Migrations.Helpers.ContentTypeManagementHelper(
                contentTypeService, dataTypeService);

            var folderId = contentTypeHelper.CreateFolder("Aubergine.Forums");
            contentTypeHelper.CreateFromFolder("~/App_Data/Aubergine/Forums/DocumentType", folderId);

        }
    }
}
