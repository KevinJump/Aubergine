using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aubergine.Core.Migrations;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;
using Umbraco.Core.Services;

namespace Aubergine.Complete.Migrations.TargetZeroOne
{

    [AubergineMigration("Complete Sample Content", Priorities.Late + Priorities.Content, "{C4C34449-0822-455E-9DC2-830150BFDCFF}")]
    public class InstallSampleContent : AubergineMigrationBase
    {
        public InstallSampleContent(ServiceContext serviceContext, ILogger logger) : base(serviceContext, logger)
        {
        }

        public override void Add()
        {
            // todo - get the sample content on a complete install...
        }

        public override void Remove()
        {
            throw new NotImplementedException();
        }
    }
}
