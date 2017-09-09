using System;
using System.Collections.Generic;
using Aubergine.Core.Migrations;
using Aubergine.Core.Migrations.Helpers;
using Umbraco.Core.Logging;
using Umbraco.Core.Services;

namespace Aubergine.Comments.Migrations
{
    [AubergineMigration("Comments DataTypes", Priorities.Primary + Priorities.DataType, "{C7AA8E6B-ACDD-4716-B6EC-9902FCBBFB0C}")]
    public class AubergineCreateDataTypes : AubergineMigrationBase
    {
        public AubergineCreateDataTypes(ServiceContext serviceContext, ILogger logger) 
            : base(serviceContext, logger)
        { }

        public override void Add()
        {
            var folderId = DataTypes.CreateFolder("Aub.Comments");

            DataTypes.Create(
                "CommentsEditor",
                "aubergine.CommentEditor",
                folderId,
                Umbraco.Core.Models.DataTypeDatabaseType.Integer,
                new Dictionary<string, string>());

            DataTypes.Create("CommentsSwitch",
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
        }

        public override void Remove()
        {
            // 
        }
    }

    [AubergineMigration("Comments Content Types", Priorities.Primary + Priorities.ContentType, "{C29E00F5-D7B7-45D5-8FC1-112702181B41}")]
    public class AubergineCreateDocTypes : AubergineMigrationBase
    {
        public AubergineCreateDocTypes(ServiceContext serviceContext, ILogger logger) 
            : base(serviceContext, logger)
        {
        }

        public override void Add()
        {
            var contentFolderId = ContentTypes.CreateFolder("Aub.Comments");

            ContentTypes.Create("pageComments_test", new ContentTypeInfo()
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

        public override void Remove()
        {
            throw new NotImplementedException();
        }
    }
}
