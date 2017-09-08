using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Umbraco.Core.IO;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using System.IO;

namespace Aubergine.Core.Migrations.Helpers
{
    /// <summary>
    ///  programatically check / create datatypes 
    /// </summary>
    public class DatatypeManagementHelper : UmbracoManagementHelperBase<IDataTypeDefinition>
    {
        private readonly IDataTypeService _dataTypeService;

        public DatatypeManagementHelper(IDataTypeService dataTypeService)
        {
            _dataTypeService = dataTypeService;
        }

        public bool Exists(string name)
        {
            var datatype = _dataTypeService.GetDataTypeDefinitionByName(name);
            return datatype != null;
        }

        public int CreateFolder(string name)
        {
            var folder = _dataTypeService.GetContainers(name, 1)
                .FirstOrDefault();

            if (folder == null)
            {
                var attempt = _dataTypeService.CreateContainer(-1, name);
                if (attempt.Success)
                {
                    LogHelper.Info<DatatypeManagementHelper>("Created Folder: {0}", () => name);
                    return attempt.Result.Entity.Id;
                }
                else
                {
                    LogHelper.Warn<DatatypeManagementHelper>("Failed to create folder: {0}", ()=> name);
                    return -1;
                }
            }
            LogHelper.Info<DatatypeManagementHelper>("Found Existing Folder: {0} {1}", () => folder.Id, () => folder.Name);
            return folder.Id;
        }


        public IDataTypeDefinition Create(string name, string propertyEditorAlias, int parentId,
            DataTypeDatabaseType databaseType)
        {
            return Create(name, propertyEditorAlias, parentId, databaseType,
                new Dictionary<string, string>());
        }

        public IDataTypeDefinition Create(string name, string propertyEditorAlias, int parentId,
            DataTypeDatabaseType databaseType, IDictionary<string, string> preValues)
        {
            if (Exists(name))
                return null;

            var dataType = new DataTypeDefinition(propertyEditorAlias)
            {
                Name = name,
                DatabaseType = databaseType,
            };

            if (parentId > 0)
                dataType.ParentId = parentId;

            _dataTypeService.Save(dataType);

            var itemPreValues = _dataTypeService.GetPreValuesCollectionByDataTypeId(dataType.Id)
                .FormatAsDictionary();


            foreach(var preValue in preValues)
            {
                LogHelper.Info<DatatypeManagementHelper>("PreValue: {0} [{1}]", () => preValue.Key, ()=> preValue.Value);

                if (itemPreValues.ContainsKey(preValue.Key))
                {
                    LogHelper.Info<DatatypeManagementHelper>("Update");
                    itemPreValues[preValue.Key].Value = preValue.Value;
                }
                else
                {
                    LogHelper.Info<DatatypeManagementHelper>("Add");
                    itemPreValues.Add(preValue.Key, new PreValue(preValue.Value));
                }
            }

            _dataTypeService.SavePreValues(dataType, itemPreValues);
            _dataTypeService.Save(dataType);

            return dataType;
        }

        public void Delete(string name)
        {
            if (Exists(name))
            {
                var dataType = _dataTypeService.GetDataTypeDefinitionByName(name);
                _dataTypeService.Delete(dataType);
            }
        }

        /// <summary>
        /// loads a bit of XML - to do the creation from.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="parentId"></param>
        public override IDataTypeDefinition Create(string filePath, int parentId)
        {
            var xml = XElement.Load(filePath);
            if (xml.Name.LocalName != "DataType")
                return default(IDataTypeDefinition);

            var name = xml.Element("Name").Value;
            var propertyEditorAlias = xml.Element("Id").Value;
            var dbType = xml.Element("DatabaseType").Value;
            var databaseType = !string.IsNullOrEmpty(dbType) ? dbType.EnumParse<DataTypeDatabaseType>(true) : DataTypeDatabaseType.Ntext;

            Dictionary<string, string> preValues = new Dictionary<string, string>();

            foreach(var element in xml.Element("PreValues").Elements("PreValue"))
            {
                var alias = element.Attribute("Alias").Value;
                var value = element.Value;
                preValues.Add(alias, value);
            }

            return Create(name, propertyEditorAlias, parentId, databaseType, preValues);
        }
    }
}
