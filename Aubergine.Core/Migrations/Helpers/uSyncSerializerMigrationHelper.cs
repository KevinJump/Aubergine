using Jumoo.uSync.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;

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
            LogHelper.Info<Aubergine>("uSync Import Helper {0}", () => folder);

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

        private ISyncSerializerWithParent<T> _treeSerializer;
        private Dictionary<T, XElement> items;

        public void ImportTree(string folder, int parentId = -1)
        {
            _treeSerializer = _serializer as ISyncSerializerWithParent<T>;
            if (_treeSerializer == null)
                return;

            items = new Dictionary<T, XElement>();

            var physicalFolder = Umbraco.Core.IO.IOHelper.MapPath(folder);
            ImportFolder(physicalFolder, parentId);

            foreach(var item in items)
            {
                _treeSerializer.DesearlizeSecondPass(item.Key, item.Value);
            }
        }

        private void ImportFolder(string physicalFolder, int parentId)
        {
            LogHelper.Info<Aubergine>("uSync Import Folder {0} [{1}]", () => physicalFolder, ()=> parentId);

            var itemId = parentId;
            if (!Directory.Exists(physicalFolder))
                return;

            var files = Directory.GetFiles(physicalFolder, "*.config");
            foreach(var file in files)
            {
                XElement node = XElement.Load(file);
                if (node != null)
                {
                    var attempt = _treeSerializer.Deserialize(node, parentId, false);
                    if (attempt.Success)
                    {
                        var contentItem = attempt.Item as IContentBase;

                        items.Add(attempt.Item, node);
                        if (contentItem != null)
                            itemId = contentItem.Id;
                    }
                }
            }

            var childFolders = Directory.GetDirectories(physicalFolder);
            foreach(var child in childFolders)
            {
                ImportFolder(child, itemId);
            }

        }
    }
}
