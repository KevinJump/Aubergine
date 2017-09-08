using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.IO;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Aubergine.Core.Migrations.Helpers
{
    /// <summary>
    ///  adds a template into umbraco, based on the assumption that it exsits on disk.
    /// </summary>
    public class TemplateManagementHelper
    {
        private readonly IFileService _fileService;

        public TemplateManagementHelper()            
        {
            _fileService = ApplicationContext.Current.Services.FileService;
        }

        public bool TemplateExists(string alias)
        {
            return _fileService.GetTemplate(alias) != null;
        }

        public bool AddTemplate(string name, string alias, string parent)
        {
            if (_fileService.GetTemplate(alias) == null)
            {
                // get the template 
                var templatePath = IOHelper.MapPath(SystemDirectories.MvcViews + "/" + alias.ToSafeFileName() + ".cshtml");
                if (!System.IO.File.Exists(templatePath))
                {
                    templatePath = IOHelper.MapPath(SystemDirectories.Masterpages + "/" + alias.ToSafeFileName() + ".master");
                    if (!System.IO.File.Exists(templatePath))
                    {
                        // cannot find the master for this..
                        templatePath = string.Empty;
                        LogHelper.Warn<TemplateManagementHelper>("Cannot find underling template file, so we cannot create the template");
                        return false;
                    }
                }
                var template = new Template(name, alias);
                template.Path = templatePath;
                template.Content = System.IO.File.ReadAllText(templatePath);

                var parentTemplate = _fileService.GetTemplate(parent);
                if (parentTemplate != null)
                {
                    template.SetMasterTemplate(parentTemplate);
                }

                _fileService.SaveTemplate(template);
            }
            return true;
        }
    }
}
