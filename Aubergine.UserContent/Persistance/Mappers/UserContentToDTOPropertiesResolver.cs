using AutoMapper;
using Aubergine.UserContent.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Aubergine.UserContent.Persistance.Mappers
{
    public class UserContentToDTOPropertiesResolver : ValueResolver<IUserContent, string>
    {
        protected override string ResolveCore(IUserContent source)
        {
            using (var sw = new StringWriter()) {
                XmlSerializer seralizer = new XmlSerializer(typeof(List<UserContentProperty>));
                seralizer.Serialize(sw, source.Properties);
                return sw.ToString();
            }
        }
    }
}
