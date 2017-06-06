using Jumoo.uSync.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace Aubergine.Core.Migrations.Helpers
{
    public class uSyncMigrationHelper
    {
        public uSyncMigrationHelper()
        {
            // initialize usync if nothing else has... 
            if (uSyncCoreContext.Instance.Serailizers == null)
                uSyncCoreContext.Instance.Init();
        }

        public void Import(string folder)
        {
            var templateHelper = new TemplateMigrationHelper();
            templateHelper.Import(folder + "/Template/");

            var dataTypeHelper = new DataTypeMigrationHelper();
            dataTypeHelper.Import(folder + "/DataType/");

            var contentTypeHelper = new ContentTypeMigrationHelper();
            contentTypeHelper.Import(folder + "/DocumentType/");
        }

        public void ImportContent(string folder)
        {
            var contentHelper = new uSyncSerializerMigrationHelper<IContent>
                (uSyncCoreContext.Instance.ContentSerializer);
            contentHelper.ImportTree(folder + "/Content/");

            var mediaHelper = new uSyncSerializerMigrationHelper<IMedia>
                (uSyncCoreContext.Instance.MediaSerializer);
            mediaHelper.ImportTree(folder + "/Media/");
        }
    }
}
