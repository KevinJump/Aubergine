using Aubergine.Forums.Models;
using AutoMapper;
using Aubergine.UserContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aubergine.Forums.Mappers
{
    public class ForumPostPropertyResolver : ValueResolver<ForumPost,IEnumerable<UserContentProperty>>
    {
        protected override IEnumerable<UserContentProperty> ResolveCore(ForumPost source)
        {
            var properties = new List<UserContentProperty>();

            properties.Add(new UserContentProperty("level", source.Level));
            properties.Add(new UserContentProperty("body", source.Body));
            return properties;
        }
    }
}
