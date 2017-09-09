using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Aubergine.Forums.Models;
using Aubergine.UserContent;
using Aubergine.UserContent.Services;
using AutoMapper;
using Umbraco.Web.WebApi;

namespace Aubergine.Forums.Controllers
{
    [Umbraco.Web.Mvc.PluginController("Aubergine")]
    [MemberAuthorize()]
    public class ForumApiController : UmbracoApiController
    {
        public IUserContentService userContentService;

        public ForumApiController()
        {
            userContentService = UserContentContext.Current
                .Instances[Forums.Instance].Service;
        }

        [HttpPut]
        public int VoteUp(Guid key)
        {
            var post = Mapper.Map<ForumPost>(userContentService.Get(key));
            if (post != null)
            {
                post.UpVotes++;
                userContentService.Save(post);
                return post.UpVotes;
            }

            return 0;
        }

        [HttpPut]
        public int VoteDown(Guid key)
        {
            var post = Mapper.Map<ForumPost>(userContentService.Get(key));
            if (post != null)
            {
                post.UpVotes++;
                userContentService.Save(post);
                return post.UpVotes;
            }

            return 0;
        }
    }
}
