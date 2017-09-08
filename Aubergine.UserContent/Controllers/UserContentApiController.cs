using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Aubergine.UserContent.Models;
using Aubergine.UserContent.Services;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace Aubergine.UserContent.Controllers
{
    [PluginController("Aubergine")]
    public class UserContentApiController : UmbracoAuthorizedApiController
    {
        [HttpGet]
        public IEnumerable<IUserContent> GetUserContentByContentId(int id, string instance = UserContent.DefaultInstance)
        {
            if (!UserContentContext.Current.Instances.ContainsKey(instance))
                return new List<IUserContent>();

            var _service = UserContentContext.Current.Instances[instance].Service;

            var items = new List<UserContentItem>();

            var content = Umbraco.TypedContent(id);
            if (content != null)
            {
                var userContent = _service.GetUserContent(content.GetKey(), true);
                return userContent;
            }

            return new List<IUserContent>();
        }

        [HttpPost]
        public Attempt<IUserContent> SaveUserContent(IUserContent content, string instance = UserContent.DefaultInstance)
        {
            if (!UserContentContext.Current.Instances.ContainsKey(instance))
                return Attempt.Fail<IUserContent>(content, new Exception("No Instance"));

            var _service = UserContentContext.Current.Instances[instance].Service;

            return _service.Save(content);
        }

        [HttpPut]
        public Attempt<int> SetStatus(Guid id, StatusData data)
        {
            if (!UserContentContext.Current.Instances.ContainsKey(data.Instance))
                return Attempt.Fail<int>(0, new Exception("No Instance"));

            var attempt = UserContentContext.Current.Instances[data.Instance].Service.UpdateStatus(id, data.Status);
            if (attempt.Success && data.PageId > 0)
            {
                var content = Services.ContentService.GetById(data.PageId);
                if (content != null)
                    UserContentContext.Current.Instances[data.Instance].Service.ClearCacheByPageKey(content.Key);
            }
            return attempt;
        }

        [HttpPut]
        public Attempt<int> ClearPageCache(Guid id, string instance = UserContent.DefaultInstance)
        {
            if (!UserContentContext.Current.Instances.ContainsKey(instance))
                return Attempt.Fail<int>(0, new Exception("No Instance"));

            UserContentContext.Current.Instances[instance].Service.ClearCacheByPageKey(id);
            return Attempt.Succeed<int>(1);
        }

        public class StatusData
        {
            public int PageId { get; set; }
            public UserContentStatus Status { get; set; }
            public string Instance { get; set; }
        }
    }
}
