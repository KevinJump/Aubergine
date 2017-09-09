using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aubergine.Core.Migrations;
using Aubergine.Core.Migrations.Helpers;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;
using Umbraco.Core.Services;

namespace Aubergine.Blog.Migrations.TargetZeroOne
{
    [AubergineMigration("Create Blog DataTypes", 
        Priorities.Standard + Priorities.DataType, "{8B0562A9-53FC-4DA8-8B31-515CF8CF0F6C}")]
    public class AubergineCreateBlogDataTypes : AubergineMigrationBase
    {
        public AubergineCreateBlogDataTypes(ServiceContext serviceContext, ILogger logger) 
            : base(serviceContext, logger)
        {
        }

        public override void Add()
        {
            var folderId = DataTypes.CreateFolder("Aub.Blogs");
            DataTypes.CreateFromFolder("~/App_Data/Aubergine/Blog/DataTypes/", folderId);
        }

        public override void Remove()
        {
            throw new NotImplementedException();
        }
    }
}
