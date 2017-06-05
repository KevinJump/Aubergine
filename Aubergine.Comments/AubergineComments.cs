using Aubergine.Core;
using Aubergine.UserContent;
using Semver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;

namespace Aubergine.Comments
{
    public class Comments : ApplicationEventHandler, IAubergineExtension
    {
        private readonly SemVersion targetVersion = new SemVersion(1, 0, 0);

        public string Name => "Aubergine Comments";
        public string ExtensionId => "{197635A4-EB21-47F6-893E-6DD7BD57724A}";
        public string Version => targetVersion.ToString();

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var migrationManager = new Aubergine.Core.Migrations.MigrationManager(applicationContext);
            migrationManager.ApplyMigration(Product.Name, targetVersion);

            // add a user content instance for storing comments
            // it is our job to ensure that the table exists 
            UserContentContext.Current.LoadInstance(Instance, Product.Table,
                applicationContext.DatabaseContext, applicationContext.ApplicationCache.RuntimeCache);

        }

        public static string Instance = "comments";
        public static string UserContentTypeAlias = "Comment";

        public static class Properties
        {
            public static string Name = "Name";
            public static string Email = "Email";
            public static string Comment = "Comment";
        }

        public static class Views
        {
            public static string CommentBox = "Comments/CommentBox";
        }

    }

    public static class Product
    {
        public const string Name = "AubComments";
        public const string Table = "UserComments";
    }



}
