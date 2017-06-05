using Aubergine.UserContent.Cache;
using Aubergine.UserContent.Config;
using Aubergine.UserContent.Persistance;
using Aubergine.UserContent.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using umbraco.interfaces;
using Umbraco.Core;
using Umbraco.Core.Cache;
using Umbraco.Core.Logging;

namespace Aubergine.UserContent
{
    public class UserContentInstance
    {
        public IUserContentService Service { get; internal set; }
        public IUserContentRepository Repository { get; internal set; }
        public IRuntimeCacheProvider Cache { get; internal set; }

        public UserContentInstance(IUserContentRepository repo, IUserContentService service, IRuntimeCacheProvider cache)
        {
            Service = service;
            Repository = repo;
            Cache = cache;
        }
    }


    public class UserContentContext
    {
        public static UserContentCacheRefresher Refresher { get; internal set; }

        public static UserContentContext Current { get; internal set; }

        public Dictionary<string, UserContentInstance> Instances { get; internal set; }

        private readonly ILogger _logger;

        public UserContentContext(ILogger logger)
        {
            _logger = logger; 
            Instances = new Dictionary<string, UserContentInstance>();
            Refresher = new UserContentCacheRefresher();
        }
        
        public static UserContentContext EnsureContext(ILogger logger)
        {
            var ctx = new UserContentContext(logger);
            Current = ctx;
            return Current;
        }

        public UserContentInstance LoadInstance(string name, string table, 
            DatabaseContext dbContext,
            IRuntimeCacheProvider cache)
        {
            if (Instances.ContainsKey(name))
                throw new Exception("Instance already exists with name " + name);

            var repo = new UserContentRepository(dbContext, table);
            var service = new UserContentService(repo, Refresher);
            var instance = new UserContentInstance(repo, service, cache);
            this.Instances.Add(name, instance);

            return instance;
        }
    }
}
