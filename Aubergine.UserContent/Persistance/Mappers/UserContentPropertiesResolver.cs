using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Aubergine.UserContent.Models;
using Aubergine.UserContent.Persistance.Models;
using AutoMapper;

namespace Aubergine.UserContent.Persistance.Mappers
{
    public class UserContentPropertiesResolver : ValueResolver<UserContentDTO,
        IEnumerable<UserContentProperty>>
    {
        protected override IEnumerable<UserContentProperty> ResolveCore(UserContentDTO source)
        {
            if (source == null)
                return null;

            XmlSerializer serializer = new XmlSerializer(
                typeof(List<UserContentProperty>));

            using (var reader = new StringReader(source.PropertyData))
            {
                return (List<UserContentProperty>)serializer.Deserialize(reader);
            }
        }
    }
}
