using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aubergine.UserContent.Persistance.Mappers
{
    public class UserContentPropertyResolver : ValueResolver<UserContentItem, object>
    {
        private readonly string propertyAlias;
        public UserContentPropertyResolver(string alias)
        {
            propertyAlias = alias;
        }

        protected override object ResolveCore(UserContentItem source)
        {
            return source.GetPropertyValue(propertyAlias, null);
        }
    }
}
