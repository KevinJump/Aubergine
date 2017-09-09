using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aubergine.Core.Migrations.Helpers;
using Umbraco.Core.Logging;
using Umbraco.Core.Services;

namespace Aubergine.Core.Migrations
{
    public abstract class AubergineMigrationBase : IAubergineMigration
    {
        protected ServiceContext Services;
        protected ILogger Logger;

        public AubergineMigrationBase(ServiceContext serviceContext, ILogger logger)
        {
            Services = serviceContext;
            Logger = logger;
        }

        public DataTypeBuilder DataTypes
        {
            get { return new DataTypeBuilder(Services.DataTypeService); }
        }

        public TemplateBuilder Templates
        {
            get { return new TemplateBuilder(Services.FileService);  }
        }

        public ContentTypeBuilder ContentTypes
        {
            get {
                return new ContentTypeBuilder(
              Services.ContentTypeService, Services.DataTypeService);
            }
        }

        public abstract void Add();

        public abstract void Remove();
    }
}
