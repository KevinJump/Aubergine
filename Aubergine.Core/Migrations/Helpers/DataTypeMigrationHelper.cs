using Jumoo.uSync.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Umbraco.Core.Models;
using Jumoo.uSync.Core.Interfaces;

namespace Aubergine.Core.Migrations.Helpers
{
    public class DataTypeMigrationHelper : uSyncSerializerMigrationHelper<IDataTypeDefinition>
    {
        public DataTypeMigrationHelper()
            : base (uSyncCoreContext.Instance.DataTypeSerializer)
        { }
    }
}
