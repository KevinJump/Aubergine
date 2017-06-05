using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Umbraco.Core.IO;

namespace Aubergine.UserContent.Config
{
    public class UserContentSettings
    {
        public UserContentSettings()
        { }

        public static UserContentSettings LoadSettings()
        { 
            try
            {
                var configFile = IOHelper.MapPath(
                    Path.Combine(SystemDirectories.Config, "usercontent.config"));

                if (System.IO.File.Exists(configFile))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(UserContentSettings));
                    string xml = File.ReadAllText(configFile);
                    using (TextReader reader = new StringReader(xml))
                    {

                        return (UserContentSettings)serializer.Deserialize(reader);
                    }
                }
            }
            catch(Exception ex)
            {

            }

            var settings = new UserContentSettings();
            settings.AutoContext = true;
            settings.ServiceType = "Aubergine.UserContent.Services.UserContentService,Aubergine.UserContent";
            settings.RepositoryType = "Aubergine.UserContent.Persistance.UserContentRepository,Aubergine.UserContent";
            settings.CacheType = "Aubergine.UserContent.Cache.UserContentCacheRefresher,Aubergine.UserContent";
            return settings;
        }

        [XmlAttribute(AttributeName = "AutoContext")]
        public bool AutoContext { get; set; }

        [XmlAttribute(AttributeName = "serviceType")]
        public string ServiceType { get; set; }
        
        [XmlAttribute(AttributeName = "repositoryType")] 
        public string RepositoryType { get; set; }

        [XmlAttribute(AttributeName = "cacheType")]
        public string CacheType { get; set; }
    }
}
