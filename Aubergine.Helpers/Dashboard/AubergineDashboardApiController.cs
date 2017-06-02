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

namespace Aubergine.Helpers.Dashboard
{
    [PluginController("Aubergine")]
    public class AubergineDashboardApiController : UmbracoAuthorizedApiController
    {
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
                
                return items;
            }

            return new List<AubergineItem>();
        }
    }
}
