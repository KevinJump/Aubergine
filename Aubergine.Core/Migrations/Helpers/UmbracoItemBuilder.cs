using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.IO;
using Umbraco.Core.Services;

namespace Aubergine.Core.Migrations.Helpers
{
    public abstract class UmbracoItemBuilder<TEntity>
    {

        public abstract TEntity Create(string filename, int parentId);

        public void CreateFromFolder(string filename, int parentId)
        {
            var folderPath = IOHelper.MapPath(filename);

            if (Directory.Exists(folderPath))
            {
                foreach (var file in Directory.GetFiles(folderPath, "*.confg", SearchOption.AllDirectories))
                {
                    Create(file, parentId);
                }
            }

        }
    }
}
