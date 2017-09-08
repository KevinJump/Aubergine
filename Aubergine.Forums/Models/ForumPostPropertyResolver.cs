using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Aubergine.UserContent;
using Aubergine.UserContent.Models;
using AutoMapper;

namespace Aubergine.Forums.Models
{
    public class ForumPostPropertyResolver : ValueResolver<ForumPost, IEnumerable<UserContentProperty>>
    {
        /// <summary>
        ///  resolves the custom properties in the ForumPost model back 
        ///  into standard IUserContent properties, meaning they get 
        ///  written back into the DB that way. 
        ///  
        ///  you could actually not do this if you set yourself up a custom 
        ///  DTO and table, as long as the fields where nullable then 
        ///  the standard repository will still work and let you put them in
        ///  but this way we keep the table structure the same as other 
        ///  usercontent.
        /// </summary>
        protected override IEnumerable<UserContentProperty> ResolveCore(ForumPost source)
        {
            var properties = source.Properties;
            properties.AddOrUpdateProperty("body", source.Body);
            return properties;
        }
    }
}
