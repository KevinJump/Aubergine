using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Umbraco.Core.IO;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using Aubergine.Core;
using Umbraco.Core;

namespace Aubergine.Helpers.Dashboard
{
    [PluginController("Aubergine")]
    public class AubergineDashboardApiController : UmbracoAuthorizedApiController
    {
        private List<InstalledItem> _installedItems;

        private AubergineDashboardApiController()
        {
            _installedItems = GetInstalled();
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

        private IEnumerable<AubergineItem> LoadJson(string name)
        {
            var localJson = IOHelper.MapPath("~/App_Plugins/Aubergine/" + name);

            if (System.IO.File.Exists(localJson))
            {
                var json = System.IO.File.ReadAllText(localJson);
                var items = JsonConvert.DeserializeObject<IEnumerable<AubergineItem>>(json);
                
                foreach(var item in items)
                {
                    item.Installed = false; 
                    var installed = _installedItems.FirstOrDefault(x => x.Id == item.Id);
                    if (installed != null)
                        item.Installed = true;
                }

                return items;
            }

            return new List<AubergineItem>();
        }

        private List<InstalledItem> GetInstalled()
        {
            var installed = new List<InstalledItem>();

            var types = TypeFinder.FindClassesOfType<IAubergineExtension>();
            foreach (var t in types)
            {
                var typeInstance = Activator.CreateInstance(t) as IAubergineExtension;
                if (typeInstance != null)
                {
                    installed.Add(new InstalledItem
                    {
                        Id = typeInstance.ExtensionId,
                        Name = typeInstance.Name
                    });
                }
            }

            return installed;

        }
    }
}
