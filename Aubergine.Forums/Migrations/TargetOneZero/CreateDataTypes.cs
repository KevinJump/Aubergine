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

namespace Aubergine.Forums.Migrations
{
    [Migration("1.0.0", 2, AubergineForums.ProductName)]
    public class CreateDataTypes : MigrationBase
    {
        public CreateDataTypes(ISqlSyntaxProvider sqlSyntax, ILogger logger) : base(sqlSyntax, logger)
        {
        }

        public override void Down()
        {
            
        }

        public override void Up()
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;
            var dataTypeHelper = new Core.Migrations.Helpers.DatatypeManagementHelper(dataTypeService);

            dataTypeHelper.CreateFromFolder("~/App_Data/Aubergine/Forums/DataType/", -1);
        }
    }
}
