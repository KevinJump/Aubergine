using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Web.Routing;

namespace Aubergine.Auth
{
    /// <summary>
    ///  default login routes, if you don't create the login/register pages within
    ///  your umbraco site, the pages will go to unfound and this contentFinder 
    ///  will direct people to teh default templates. 
    /// </summary>
    public class AuthContentFinder : IContentFinder
    {
        public bool TryFindContent(PublishedContentRequest contentRequest)
        {
            if (contentRequest == null)
                return false;

            var url = contentRequest.Uri.AbsolutePath.TrimEnd(new char[] { '/', ' ' });
            LogHelper.Info<AuthContentFinder>("url: {0}", () => url);

            switch (url.ToLower())
            {
                case AuthUrls.Login:
                    contentRequest.PublishedContent = GetHomepage(contentRequest.RoutingContext);
                    contentRequest.TrySetTemplate(TemplateAliases.Login);
                    break;
                case AuthUrls.Register:
                    contentRequest.PublishedContent = GetHomepage(contentRequest.RoutingContext);
                    contentRequest.TrySetTemplate(TemplateAliases.Register);
                    break;
                case AuthUrls.Reset:
                    contentRequest.PublishedContent = GetHomepage(contentRequest.RoutingContext);
                    contentRequest.TrySetTemplate(TemplateAliases.Reset);
                    break;
                case AuthUrls.Verify:
                    contentRequest.PublishedContent = GetHomepage(contentRequest.RoutingContext);
                    contentRequest.TrySetTemplate(TemplateAliases.Verify);
                    break;
            }

            return contentRequest.PublishedContent != null;
        }

        private IPublishedContent GetHomepage(RoutingContext routeContext)
        {
            // get the sitehomepage 
            var rootNodes = routeContext.UmbracoContext.ContentCache.GetAtRoot();
            var site = rootNodes.FirstOrDefault(x => x.DocumentTypeAlias == "Homepage");
            if (site == null)
                site = rootNodes.FirstOrDefault();

            return site;

        }
    }
}
