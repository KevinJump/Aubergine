using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aubergine.Core;
using Aubergine.Forums.Models;
using Aubergine.UserContent;
using Aubergine.UserContent.Models;
using Aubergine.UserContent.Persistance.Mappers;
using Aubergine.UserContent.Persistance.Models;
using AutoMapper;
using Semver;
using Umbraco.Core;
using Umbraco.Web.Routing;

namespace Aubergine.Forums
{
    public class AubergineForums : IAubergineExtension
    {
        public const string ProductName = "AubergineForums";
        public const string ProductVersion = "1.0.0";
        public const string Table = "Aubergine_Forums";
        public const string Instance = "Forums";
        public const string UserContentTypeAlias = "Post";
        

        // //////
        public string Name => ProductName;
        public string Version => ProductVersion;
        public string ExtensionId => "{BD379DAA-B78A-4A04-909B-91DFA791B83B}";
       
    }

    public class AubergineForumsEventHandler : ApplicationEventHandler
    {

        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ContentFinderResolver.Current.InsertTypeBefore<ContentFinderByNotFoundHandlers, ForumContentFinder>();
        }

        protected override void ApplicationStarted(
            UmbracoApplicationBase umbracoApplication, 
            ApplicationContext applicationContext)
        {

            var migrationHelper = new Aubergine.Core.MigrationManager(applicationContext);
            migrationHelper.ApplyMigration(AubergineForums.ProductName, SemVersion.Parse(AubergineForums.ProductVersion));

            // currently not sure why the IncludeBase calls are not working
            // so we are explitly adding the ForMember (but it should be inherited?)

            Mapper.CreateMap<ForumPostsDTO, ForumPost>()
                // .IncludeBase<UserContentDTO, UserContentItem>()
                .ForMember(x => x.Properties, opt => opt.ResolveUsing(new UserContentPropertiesResolver()));


            Mapper.CreateMap<ForumPost, ForumPostsDTO>()
                // .IncludeBase<UserContentItem, UserContentDTO>()
                .ForMember(x => x.PropertyData, opt => opt.ResolveUsing(new UserContentToDTOPropertiesResolver()));

            /// a mapper for the curtom thing.
            Mapper.CreateMap<IUserContent, ForumPost>()
                .ForMember(x => x.Body,
                    opt => opt.ResolveUsing(new UserContentPropertyResolver("body")));


            /// the forum instance is it's own table but with a couple of 
            /// extra columns - so we load the instance with our own (inherited) models.
            /// 
            // for this to work it needs the 
            UserContentContext.Current.LoadInstance<ForumPost, ForumPostsDTO>(
                AubergineForums.Instance,
                AubergineForums.Table,
                applicationContext.DatabaseContext, 
                applicationContext.ApplicationCache.RuntimeCache);
        }
    }
}
