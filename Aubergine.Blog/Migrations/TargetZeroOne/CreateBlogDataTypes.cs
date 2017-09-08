using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aubergine.Core.Migrations.Helpers;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;

namespace Aubergine.Blog.Migrations.TargetZeroOne
{
    [Migration("1.0.0", 2, AubergineBlog.ProductName)]
    public class CreateBlogDataTypes : MigrationBase
    {
        protected CreateBlogDataTypes(ISqlSyntaxProvider sqlSyntax, ILogger logger) : base(sqlSyntax, logger)
        {
        }

        public override void Down()
        {

        }

        public override void Up()
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;
            var dataTypeHelper = new DatatypeManagementHelper(dataTypeService);

            var folderId = dataTypeHelper.CreateFolder("Aub.Blogs");

            dataTypeHelper.CreateFromFolder("~App_Data/Aubergine/Blog/DataTypes/", folderId);
        }
    }
}
