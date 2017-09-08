using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Aubergine.UserContent.Models;
using AutoMapper;

namespace Aubergine.UserContent.Persistance.Mappers
{
    public class UserContentToDTOPropertiesResolver 
        : ValueResolver<IUserContent, string>
    {
        protected override string ResolveCore(IUserContent source)
        {
            using(var writer = new StringWriter())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<UserContentProperty>));
                serializer.Serialize(writer, source.Properties);
                return writer.ToString();
            }
        }
    }
}
