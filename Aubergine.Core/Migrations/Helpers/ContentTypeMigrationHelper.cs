using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using System.Xml.Linq;
using Jumoo.uSync.Core;

namespace Aubergine.Core.Migrations.Helpers
{
    public class ContentTypeMigrationHelper : uSyncSerializerMigrationHelper<IContentType>
    {
        public readonly IDataTypeService _dataTypeService; 
        public ContentTypeMigrationHelper()
            : base (uSyncCoreContext.Instance.ContentTypeSerializer)
        {
            _dataTypeService = ApplicationContext.Current.Services.DataTypeService;
        }

        /// <summary>
        ///  Adds a property to the contenttype item.
        /// </summary>
        public void AddProperty(IContentTypeBase item, string alias, string name, string tab, string editorAlias,
            string description)
        {
            // return if the property already exists.
            if (item.PropertyTypes.Any(x => x.Alias == alias))
                return;

            var dataTypeDef = _dataTypeService.GetDataTypeDefinitionByPropertyEditorAlias(editorAlias)
                .FirstOrDefault();

            if (dataTypeDef != null)
            {
                // add a property 
                var propertyType = new PropertyType(dataTypeDef, alias);

                propertyType.Name = name;
                propertyType.Description = description;
                item.AddPropertyType(propertyType, tab);
            }
        }
    }
}
