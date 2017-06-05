using Aubergine.UserContent.Models;
using Aubergine.UserContent.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Umbraco.Core.Logging;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace Aubergine.UserContent.Controllers
{
    /// <summary>
    /// Back office controller, for managing User Content via dashboards and
    /// propertyeditors.
    /// </summary>
    [PluginController("Aubergine")]
    public class UserContentApiController : UmbracoAuthorizedApiController
    {
        [HttpGet]
        public IEnumerable<UserContentItem> GetUserContent(int id, string type, string instance = "default")
        {
            if (!UserContentContext.Current.Instances.ContainsKey(instance))
                return null;

            var _service = UserContentContext.Current.Instances[instance].Service;

            var items = new List<UserContentItem>();
            var node = Umbraco.TypedContent(id);
            if (node != null)
            {
                var content = _service.GetByContentKey(node.GetKey(), true)
                    .Where(x => x.UserContentType == type)
                    .ToList();
                
                items.AddRange(content.ConvertAll<UserContentItem>(o => (UserContentItem)o));
            }

            return items;
        }

        [HttpGet]
        public IEnumerable<UserContentItem> GetUserContent(int id, string instance = "default")
        {
            if (!UserContentContext.Current.Instances.ContainsKey(instance))
                return null;

            var _service = UserContentContext.Current.Instances[instance].Service;

            var items = new List<UserContentItem>();
            var node = Umbraco.TypedContent(id);
            if (node != null)
            {
                LogHelper.Info<UserContentApiController>("node: {0}", () => node.GetKey());
                var content = _service.GetByContentKey(node.GetKey(), true).ToList();
                items.AddRange(content.ConvertAll<UserContentItem>(o => (UserContentItem)o));
            }

            return items;

        }

        [HttpGet]
        public IUserContent GetUserContent(Guid key, string instance = "default")
        {
            if (!UserContentContext.Current.Instances.ContainsKey(instance))
                return null;

            return UserContentContext.Current.Instances[instance].Service.Get(key);
        }


        [HttpGet]
        public bool DeleteUserContent(Guid key, string instance = "default")
        {
            if (!UserContentContext.Current.Instances.ContainsKey(instance))
                return false;

            var _service = UserContentContext.Current.Instances[instance].Service;

            var content = _service.Get(key);
            if (content != null)
            {
                var attempt = _service.Delete(content);
                return attempt.Success;
            }

            return false;
        }

        [HttpPost]
        public bool SaveUserContent(IUserContent content, string instance = "default")
        {
            if (!UserContentContext.Current.Instances.ContainsKey(instance))
                return false;

            return UserContentContext.Current.Instances[instance].Service.Save(content).Success;
        }


        [System.Web.Http.HttpGet]
        public bool SetStatus(Guid id, UserContentStatus status, string instance = "default")
        {
            if (!UserContentContext.Current.Instances.ContainsKey(instance))
                return false;

            return UserContentContext.Current.Instances[instance].Service.SetStatus(id, status);
        }
        
    }
}
