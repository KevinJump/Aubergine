using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Services;
using Umbraco.Core.Models;
using Umbraco.Core;
using Umbraco.Core.Logging;
using System.Xml.Linq;

namespace Aubergine.Core.Migrations.Helpers
{
    public class ContentTypeManagementHelper : UmbracoManagementHelperBase<IContentType>
    {
        private readonly IContentTypeService _contentTypeService;
        private readonly IDataTypeService _dataTypeService;

        public ContentTypeManagementHelper(
            IContentTypeService contentTypeService,
            IDataTypeService dataTypeService)
        {
            _dataTypeService = dataTypeService;
            _contentTypeService = contentTypeService;
        }

        public bool Exists(string alias)
        {
            var contentType = _contentTypeService.GetContentType(alias);
            return contentType != null;
        }

        public int CreateFolder(string name)
        {
            var folder = _contentTypeService.GetContentTypeContainers(name, 1)
                .FirstOrDefault();

            if (folder == null)
            {
                var attempt = _contentTypeService.CreateContentTypeContainer(-1, name);
                if (attempt.Success)
                {
                    LogHelper.Info<ContentTypeManagementHelper>("Created Folder: {0} [{1}]", () => name, () => attempt.Result.Entity.Id);
                    return attempt.Result.Entity.Id;
                }
                else
                {
                    LogHelper.Warn<ContentTypeManagementHelper>("Failed Created Folder: {0}", () => name);
                    return -1;
                }
            }
            LogHelper.Info<ContentTypeManagementHelper>("Found Existing: [{0}] {1}", () => folder.Id, () => folder.Name);
            return folder.Id;
        }

        public IContentType Create(string alias, ContentTypeInfo info)
        {
            if (Exists(alias))
                return null;

            int parentId = -1;
            if (info.MasterId > 0)
            {
                // get the master page by id (only usefull if we've just created it!)
                parentId = info.MasterId;
            }
            else if (!info.Master.IsNullOrWhiteSpace())
            {
                // find the master by name (will work for contenttypes but not folders!)
                var parent = _contentTypeService.GetContentType(info.Master);
                if (parent != null)
                {
                    parentId = parent.Id;
                }
            }

            LogHelper.Info<ContentTypeManagementHelper>
                ("Creating Content Type: {0} [{1}]", () => alias, () => parentId);

            var contentType = new ContentType(parentId)
            {
                Alias = alias,
                Name = info.Name,
                Key = info.Key,
                Icon = info.Icon,
                Thumbnail = info.Thumbnail,
                Description = info.Description,
                AllowedAsRoot = info.AllowAtRoot
            };

            CreateStructure(contentType, info.Allowed);
            CreateCompositions(contentType, info.Compositions);
            CreateTemplates(contentType, info.Templates);

            CreateProperties(contentType, info.Tabs);

            _contentTypeService.Save(contentType);

            SortTabs(contentType, info.Tabs);
            _contentTypeService.Save(contentType);

            return contentType;
        }

        public void Delete(string alias)
        {
            var contentType = _contentTypeService.GetContentType(alias);
            if (contentType != null)
            {
                _contentTypeService.Delete(contentType);
            }
        }

        private void CreateProperties(ContentType item, IEnumerable<ContentTypeTab> tabs)
        {
            foreach (var tab in tabs)
            {
                foreach (var property in tab.Properties)
                {
                    LogHelper.Info<ContentTypeManagementHelper>("Looking for DocType: {0}", () => property.DataType);

                    var dataType = _dataTypeService.GetDataTypeDefinitionByName(property.DataType);

                    var itemProperty = item.PropertyTypes.SingleOrDefault(x => x.Alias == property.Alias);
                    if (itemProperty == null)
                    {
                        // create it.
                        itemProperty = new PropertyType(dataType, property.Alias)
                        {
                            Name = property.Name
                        };

                        if (!property.Description.IsNullOrWhiteSpace())
                            itemProperty.Description = property.Description;

                        if (!property.Validation.IsNullOrWhiteSpace())
                            itemProperty.ValidationRegExp = property.Validation;

                        itemProperty.Mandatory = property.Mandatory;
                        itemProperty.SortOrder = property.SortOrder;
                        itemProperty.PropertyEditorAlias = dataType.PropertyEditorAlias;
                        item.AddPropertyType(itemProperty, tab.TabName);
                    }
                }
            }
        }
        private void SortTabs(ContentType item, IEnumerable<ContentTypeTab> tabs)
        {
            foreach (var tab in tabs)
            {
                var itemTab = item.PropertyGroups.FirstOrDefault(x => x.Name == tab.TabName);
                if (itemTab != null)
                {
                    itemTab.SortOrder = tab.SortOrder;
                }
            }
        }

        private void CreateStructure(ContentType item, Dictionary<Guid, string> doctypes)
        {
            var allowedTypes = new List<ContentTypeSort>();
            int sortOrder = 0;
            foreach(var doctype in doctypes)
            {
                var contentType = _contentTypeService.GetContentType(doctype.Value);
                if (contentType != null)
                {
                    allowedTypes.Add(new ContentTypeSort(
                        new Lazy<int>(() => contentType.Id), sortOrder, contentType.Name));
                    sortOrder++;
                }
            }

            item.AllowedContentTypes = allowedTypes;
        }

        private void CreateCompositions(ContentType item, Dictionary<Guid, string> compositions)
        {
            var comps = new List<IContentTypeComposition>();
            foreach(var comp in compositions)
            {
                IContentType type = null;
                if (comp.Key != null && comp.Key != Guid.Empty)
                    type = _contentTypeService.GetContentType(comp.Key);
                else
                    type = _contentTypeService.GetContentType(comp.Value);

                if (type != null)
                    comps.Add(type);
            }

            item.ContentTypeComposition = comps;
        }

        private void CreateTemplates(ContentType item, List<string> templates)
        {
            List<ITemplate> itemTemplates = new List<ITemplate>();
            if (templates.Any()) {
                var _fileService = ApplicationContext.Current.Services.FileService;

                foreach (var template in templates)
                {
                    var t = _fileService.GetTemplate(template);
                    if (t != null)
                        itemTemplates.Add(t);
                }
            }

            item.AllowedTemplates = itemTemplates;
        }

        public override IContentType Create(string filename, int parentId)
        {
            var xml = XElement.Load(filename);
            if (xml.Name.LocalName != "DocumentType")
                return null;

            var info = xml.Element("Info");
            var alias = info.Element("Alias").Value;

            var contentTypeOptions = new ContentTypeInfo()
            {
                Key = Guid.Parse(info.Element("Key").Value),
                Name = info.Element("Name").Value,
                AllowAtRoot = bool.Parse(info.Element("AllowAtRoot").Value),
                Description = info.Element("Description").Value,
                Icon = info.Element("Icon").Value,
                Thumbnail = info.Element("Thumbnail").Value,
            };

            var compXml = info.Element("Compositions");
            foreach (var compositionXml in compXml.Elements("Composition"))
            {
                contentTypeOptions.Compositions.Add(
                    Guid.Parse(compositionXml.Attribute("Key").Value),
                    compositionXml.Value);
            }

            var structXml = xml.Element("Structure");
            foreach (var doctype in structXml.Elements("DocumentType"))
            {
                contentTypeOptions.Allowed.Add(
                    Guid.Parse(doctype.Attribute("Key").Value),
                    doctype.Value);
            }

            var templatesXml = xml.Element("AllowedTemplates");
            foreach (var template in templatesXml.Elements("Template"))
            {
                contentTypeOptions.Templates.Add(template.Value);
            }

            var propertiesXml = xml.Element("GenericProperties");
            foreach (var propXml in propertiesXml.Elements("GenericProperty"))
            {
                var tabName = propXml.Element("Tab").Value;
                var tab = contentTypeOptions.Tabs.FirstOrDefault(x => x.TabName == tabName);
                if (tab == null)
                {
                    tab = new ContentTypeTab() { TabName = tabName };
                    contentTypeOptions.Tabs.Add(tab);
                }

                tab.Properties.Add(new ContentTypeProperty()
                {
                    Alias = propXml.Element("Alias").Value,
                    Mandatory = bool.Parse(propXml.Element("Mandatory").Value),
                    Name = propXml.Element("Name").Value,
                    Description = propXml.Element("Description").Value,
                    SortOrder = int.Parse(propXml.Element("SortOrder").Value),
                    Validation = propXml.Element("Validation").Value,
                    DataType = propXml.Element("Type").Value
                });

            }

            return Create(alias, contentTypeOptions);

        }
    }


    public class ContentTypeInfo
    {
        public Guid Key { get; set; }

        public string Name { get; set; }
        public string Icon { get; set; }
        public string Thumbnail { get; set; }
        public string Description { get; set; }
        public bool AllowAtRoot { get; set; }
        public string Master { get; set; }
        public int MasterId { get; set; }

        public Dictionary<Guid, string> Allowed { get; set; }
        public Dictionary<Guid, string> Compositions { get; set; }
        public List<ContentTypeTab> Tabs { get; set; }
        public List<string> Templates { get; set; }

        public ContentTypeInfo()
        {
            Allowed = new Dictionary<Guid, string>();
            Compositions = new Dictionary<Guid, string>();
            Tabs = new List<ContentTypeTab>();
            Templates = new List<string>();
        }
    }

    public class ContentTypeTab
    {
        public string TabName { get; set; }
        public int SortOrder { get; set; }

        public List<ContentTypeProperty> Properties { get; set; }

        public ContentTypeTab()
        {
            Properties = new List<ContentTypeProperty>();
        }
    }

    public class ContentTypeProperty
    {
        public Guid Key { get; set; }

        public string Name { get; set; }
        public string Alias { get; set; }
        public string DataType { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }

        public bool Mandatory { get; set; }
        public string Validation { get; set; }
    }
}
