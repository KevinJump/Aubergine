using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Aubergine.Core;
using Newtonsoft.Json;
using Umbraco.Core;
using Umbraco.Core.IO;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace Aubergine.Helpers.Controllers
{
    [PluginController("Aubergine")]
    public class AubergineDashboardApiController
        : UmbracoAuthorizedApiController
    {
        private IEnumerable<InstalledItem> _installedItems;

        private AubergineDashboardApiController()
        {
            _installedItems = LoadInstalled();
        }

        [HttpGet]
        public IEnumerable<AubergineItem> GetFeatures()
        {
            return LoadJson("features.json");
        }

        [HttpGet]
        public IEnumerable<AubergineItem> GetContent()
        {
            return LoadJson("content.json");
        }

        private IEnumerable<AubergineItem> LoadJson(string fileName)
        {
            var localPath = IOHelper.MapPath("~/App_Plugins/Aubergine/" + fileName);

            if (System.IO.File.Exists(localPath))
            {
                var json = System.IO.File.ReadAllText(localPath);
                var items = JsonConvert.DeserializeObject<IEnumerable<AubergineItem>>(json);

                foreach(var item in items)
                {
                    item.Installed = _installedItems.Any(x => x.Id == item.Id);
                }

                return items;
            }

            return Enumerable.Empty<AubergineItem>();
        }

        private IEnumerable<InstalledItem> LoadInstalled()
        {
            var installed = new List<InstalledItem>();

            var types = TypeFinder.FindClassesOfType<IAubergineExtension>();
            foreach(var t in types)
            {
                var instance = Activator.CreateInstance(t) as IAubergineExtension;
                if (instance != null)
                {
                    installed.Add(new InstalledItem
                    {
                        Id = instance.ExtensionId,
                        Name = instance.Name
                    });
                }
            }


            return installed;
        }
    }

    public class AubergineItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Depends { get; set; }
        public string Nuget { get; set; }
        public string Package { get; set; }
        public string Link { get; set; }

        public bool Enabled { get; set; }
        public bool Installed { get; set; }
    }

    public class InstalledItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
