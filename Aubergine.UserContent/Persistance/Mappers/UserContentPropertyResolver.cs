using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Aubergine.UserContent.Models;
using Aubergine.UserContent.Persistance.Models;
using AutoMapper;

namespace Aubergine.UserContent.Persistance.Mappers
{
    /// <summary>
    ///  Maps an item out of the Generic Properties object into a specfic value
    /// </summary>
    public class UserContentPropertyResolver : ValueResolver<IUserContent, object>
    {
        private readonly string propertyAlias;

        public UserContentPropertyResolver(string alias)
        {
            propertyAlias = alias;
        }

        protected override object ResolveCore(IUserContent source)
        {
            // extra the value we want from the base mapping.
            return source.GetPropertyValue(propertyAlias, null);
        }
    }
}
