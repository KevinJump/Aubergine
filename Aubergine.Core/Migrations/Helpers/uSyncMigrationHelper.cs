using Jumoo.uSync.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
