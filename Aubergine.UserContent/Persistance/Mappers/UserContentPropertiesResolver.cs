using AutoMapper;
using Aubergine.UserContent.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Umbraco.Core.Models;

namespace Aubergine.UserContent.Persistance.Mappers
{
    public class UserContentPropertiesResolver : ValueResolver<UserContentDTO, IEnumerable<UserContentProperty>>
    {
        protected override IEnumerable<UserContentProperty> ResolveCore(UserContentDTO source)
        {
            XmlSerializer seralizer = new XmlSerializer(typeof(List<UserContentProperty>));
            using (var reader = new StringReader(source.PropertyData))
            {
                return (List<UserContentProperty>)seralizer.Deserialize(reader);
            }
            
        }
    }

}
