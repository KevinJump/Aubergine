using Aubergine.UserContent.Models;
using Aubergine.UserContent.Persistance.Models;
using Aubergine.UserContent.Persistance;
using Aubergine.UserContent.Services;
using Umbraco.Core.Cache;

namespace Aubergine.UserContent
{
    // base Interface, is what we store in the list
    public interface IUserContentInstance
    {
        IUserContentService Service { get; }
        IUserContentRepository Repository { get; }
        IRuntimeCacheProvider Cache { get; }
    }
}
