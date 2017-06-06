using Jumoo.uSync.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Aubergine.Core.Migrations.Helpers
{
    public class uSyncSerializerMigrationHelper<T>
    {
        private readonly ISyncSerializer<T> _serializer;

        public uSyncSerializerMigrationHelper(ISyncSerializer<T> serializer)
        {
            _serializer = serializer;
        }

        public void Import(string folder)
        {
            if (_serializer == null)
                return;

            var physicalFolder = Umbraco.Core.IO.IOHelper.MapPath(folder);
            if (!Directory.Exists(physicalFolder))
                return;

            var files = Directory.GetFiles(physicalFolder, "*.config", SearchOption.AllDirectories);

            Dictionary<T, XElement> items = new Dictionary<T, XElement>();

            foreach (var file in files)
            {
                XElement node = XElement.Load(file);
                if (node != null)
                {
                    var attempt = _serializer.DeSerialize(node, false);
                    if (attempt.Success && attempt.Item != null)
                    {
                        items.Add(attempt.Item, node);
                    }
                }
            }

            if (_serializer as ISyncSerializerTwoPass<T> != null)
            {
                var twoPassSerializer = _serializer as ISyncSerializerTwoPass<T>;
                foreach (var item in items)
                {
                    twoPassSerializer.DesearlizeSecondPass(item.Key, item.Value);
                }
            }

        }
    }
}
