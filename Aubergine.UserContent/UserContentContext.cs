using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aubergine.UserContent.Cache;
using Aubergine.UserContent.Persistance;
using Aubergine.UserContent.Services;
using umbraco.interfaces;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Cache;
using Umbraco.Core.Persistence;
using Aubergine.UserContent.Persistance.Models;
using Aubergine.UserContent.Models;
using Umbraco.Core.Events;

namespace Aubergine.UserContent
{

    public class UserContentInstance<TUserContent, TUserContentDTO> : IUserContentInstance
        where TUserContentDTO : UserContentDTO
        where TUserContent : IUserContent
    {
        public IUserContentService Service { get; internal set; }
        public IUserContentRepository Repository { get; internal set; }
        public IRuntimeCacheProvider Cache { get; internal set; }

        internal UserContentInstance(DatabaseContext databaseContext,
            string tableName,
            ICacheRefresher refresher,
            IRuntimeCacheProvider cacheProvider)
        {
            Repository = new UserContentRepository<TUserContent, TUserContentDTO>(databaseContext, tableName);
            Service = new UserContentService<TUserContent, TUserContentDTO>(
                (UserContentRepository<TUserContent, TUserContentDTO>)Repository, refresher);
            Cache = cacheProvider;
        }
    }

    public class UserContentContext
    {
        public static UserContentContext Current { get; internal set; }
        public Dictionary<string, IUserContentInstance> Instances { get; internal set; }
        internal readonly ICacheRefresher refresher;

        private ILogger _logger;

        internal static void EnsureContext(
            ILogger logger,
            IRuntimeCacheProvider runtimeCacheProvider
            )
        {
            Current = new UserContentContext(logger, runtimeCacheProvider);
        }

        private UserContentContext(
            ILogger logger,
            IRuntimeCacheProvider runtimeCacheProvider)
        {
            _logger = logger;

            Instances = new Dictionary<string, IUserContentInstance>();
            refresher = new UserContentCacheRefresher(runtimeCacheProvider);
        }

        public void LoadInstance<TUserContent, TUserContentDTO>(
            string name, string table,
            DatabaseContext databaseContext,
            IRuntimeCacheProvider cacheProvider
            )
            where TUserContentDTO : UserContentDTO
            where TUserContent : IUserContent
        {
            if (Instances.ContainsKey(name))
                throw new Exception("Instance already exists with name " + name);

            Instances.Add(name, 
                new UserContentInstance<TUserContent, TUserContentDTO>(databaseContext, table, refresher, cacheProvider));

        }

        private void EnsureContentTable(DatabaseContext databaseContext, string tableName)
        {
            var dbHelper = new DatabaseSchemaHelper(databaseContext.Database, _logger, databaseContext.SqlSyntax);

            if (!dbHelper.TableExist(tableName))
            {
                // create the table here... but we need to do it with diffrent table name.
                // 
            }

        }
    }
}
