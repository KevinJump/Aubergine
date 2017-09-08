using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aubergine.Core.Migrations.Helpers;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;

namespace Aubergine.Comments.Migrations
{
    [Migration("1.0.0", 2, Comments.ProductName)]
    public class CreateDataAndDocTypes : MigrationBase
    {
        public CreateDataAndDocTypes(ISqlSyntaxProvider sqlSyntax, ILogger logger) 
            : base(sqlSyntax, logger)
        {
        }

        public override void Down()
        {
            // 
        }

        public override void Up()
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;
            var contentTypeService = ApplicationContext.Current.Services.ContentTypeService;

            //
            // Create the Datatypes (Allow Comments) and (Comments Moderator)
            // -- we can then use these for the comments doctype. 
            //
            var dataTypeHelper = new DatatypeManagementHelper(dataTypeService);

            var folderId = dataTypeHelper.CreateFolder("Aub.Comments");

            dataTypeHelper.Create(
                "CommentsEditor",
                "aubergine.CommentEditor",
                folderId,
                Umbraco.Core.Models.DataTypeDatabaseType.Integer,
                new Dictionary<string, string>());

            dataTypeHelper.Create("CommentsSwitch",
                "Our.Umbraco.Switcher",
                folderId,
                Umbraco.Core.Models.DataTypeDatabaseType.Integer,
                new Dictionary<string, string>()
                {
                    { "hideLabel", "" },
                    { "switchOn", "1" },
                    { "showIcons", "" },
                    { "statusLeftRight", "" },
                    { "onLabelText", "Allow Comments" },
                    { "offLabelText", "No Comments" },
                    { "switchClass", "blue" },
                });


            //
            // Create the Content Type (pageComments) this is the 
            // content type that you can then add to any page to 
            // turn comments on (assuming the template has the action)
            //

            var contentTypeHelper = new ContentTypeManagementHelper(
                contentTypeService, dataTypeService);

            var contentFolderId = contentTypeHelper.CreateFolder("Aub.Comments");

            contentTypeHelper.Create("pageComments_test", new ContentTypeInfo()
            {
                MasterId = contentFolderId,
                Name = "Page Comments_test",
                Master = "Elements",
                AllowAtRoot = false,
                Description = "Adds the option for commenting to the page",
                Icon = "icon-chat-active",
                Tabs = new List<ContentTypeTab>()
                {
                    new ContentTypeTab()
                    {
                        TabName = "Comments",
                        SortOrder = 160,
                        Properties = new List<ContentTypeProperty>()
                        {
                            new ContentTypeProperty
                            {
                                Alias = "allowComments",
                                Name = "Allow Comments",
                                DataType = "CommentsSwitch",
                                Description = "turn comments on or off for the page",
                                SortOrder = 0,
                            },
                            new ContentTypeProperty
                            {
                                Alias = "comments",
                                Name = "Comments",
                                DataType = "CommentsEditor",
                                Description = "View of comments",
                                SortOrder = 1
                            }
                        }
                    }
                }
            });

            
        }
    }
}
