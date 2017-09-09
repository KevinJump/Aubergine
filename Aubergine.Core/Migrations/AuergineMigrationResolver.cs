using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.ObjectResolution;
using Umbraco.Core.Services;

namespace Aubergine.Core.Migrations
{
    internal class AubergineMigrationResolver
        : LazyManyObjectsResolverBase<AubergineMigrationResolver, IAubergineMigration>
    {
        public AubergineMigrationResolver(IServiceProvider service, ILogger logger,
            Func<IEnumerable<Type>> items)  :
            base (service, logger, items)
        { }

        public IEnumerable<IAubergineMigration> Configurations
        {
            get { return Values; }
        }
    }

    // our service provider, so we can start up our object.s
    internal class AubergineServiceProvider : IServiceProvider
    {
        private readonly ApplicationContext _applicationContext;

        public AubergineServiceProvider(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;           
        }

        public object GetService(Type serviceType)
        {
            var argList = new[] { typeof(ServiceContext), typeof(ILogger) };
            var found = serviceType.GetConstructor(argList);
            if (found != null)
            {
                return found.Invoke(new Object[]
                {
                    _applicationContext.Services,
                    _applicationContext.ProfilingLogger.Logger
                });
            }

            return Activator.CreateInstance(serviceType);
        }
    }
}
