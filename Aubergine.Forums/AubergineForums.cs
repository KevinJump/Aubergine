using Aubergine.Core;
using Aubergine.Forums.Mappers;
using Aubergine.Forums.Models;
using Aubergine.UserContent;
using Aubergine.UserContent.Persistance.Mappers;
using AutoMapper;
using Semver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Web.Routing;

namespace Aubergine.Forums
{
    public class AubergineForums : ApplicationEventHandler, IAubergineExtension
    {
        SemVersion targetVersion = new SemVersion(1, 0, 0);

        public string Name => "Forums";
        public string ExtensionId => "{BA785FF0-28C3-4BA0-A5D2-68D5D91C7CF1}";
        public string Version => targetVersion.ToString();

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            // setup the migration stuff (this creates the forums table)
            var migrationManager = new Aubergine.Core.Migrations.MigrationManager(applicationContext);
            migrationManager.ApplyMigration(Product.Name, targetVersion);

            // sets up the forums instance (because we have a table)
            UserContentContext.Current.LoadInstance(
                Product.Instance,
                Product.Table,
                applicationContext.DatabaseContext,
                applicationContext.ApplicationCache.RuntimeCache);
        }


        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {

            ContentFinderResolver.Current.InsertTypeBefore<ContentFinderByNotFoundHandlers, ForumContentFinder>();

            // automapper some things from UserContentItem to a forum post, 
            // this is just some shorthand so we don't have to keep doing 'GetPropertyValue' 
            // 
            Mapper.CreateMap<UserContentItem, ForumPost>()
                .ForMember(x => x.Level,
                    opt => opt.ResolveUsing(new UserContentPropertyResolver("level")))
                .ForMember(x => x.Body,
                    opt => opt.ResolveUsing(new UserContentPropertyResolver("body")));

            Mapper.CreateMap<ForumPost, UserContentItem>()
                .ForMember(x => x.Properties,
                    opt => opt.ResolveUsing(new ForumPostPropertyResolver()));
        }

    }


    public class Product
    {
        public const string Name = "AubForums";
        public const string Table = "UserForums";
        public const string Instance = "Forums";

        public const string UserContentTypeAlias = "Post";
    }
}
