using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aubergine.Core.Migrations;
using Aubergine.Core.Migrations.Helpers;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;
using Umbraco.Core.Services;

namespace Aubergine.Blog.Migrations.TargetZeroOne
{
    [AubergineMigration(
        "Create Blog Content Types", 
        (Priorities.Standard + Priorities.ContentType)
        , "{6730A7E4-10D7-4174-AB0F-BB77A506F581}")]
    public class AubergineCreateBlogContentTypes : AubergineMigrationBase
    {
        public AubergineCreateBlogContentTypes(ServiceContext serviceContext, ILogger logger) 
            : base(serviceContext, logger)
        {
        }

        public override void Add()
        {
            var folderId = ContentTypes.CreateFolder("Aub.Blog");
            ContentTypes.CreateFromFolder("~/App_Data/Aubergine/Blog/DocumentType/", folderId);
        }

        public override void Remove()
        {
            throw new NotImplementedException();
        }
    }

}
