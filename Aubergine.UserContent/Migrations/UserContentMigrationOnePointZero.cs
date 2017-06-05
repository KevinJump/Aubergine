using Aubergine.UserContent.Models;
using Aubergine.UserContent.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;

namespace Aubergine.UserContent.Migrations
{
    [Migration("0.0.1", 1, "UserContent")]
    public class UserContentMigrationOnePointZero : MigrationBase
    {
        private readonly UmbracoDatabase _database =
            ApplicationContext.Current.DatabaseContext.Database;

        private readonly DatabaseSchemaHelper _schemaHelper;

        public UserContentMigrationOnePointZero(ISqlSyntaxProvider sqlSyntax, ILogger logger) 
            : base(sqlSyntax, logger)
        {
            _schemaHelper = new DatabaseSchemaHelper(_database, logger, sqlSyntax);
        }

        public override void Down()
        {
            if (_schemaHelper.TableExist("UserContnet"))
                Delete.Table("UserContent");
        }

        public override void Up()
        {
            if(!_schemaHelper.TableExist("UserContent"))
            {
                _schemaHelper.CreateTable<UserContentDTO>(false);
            }
        }
    }
}
