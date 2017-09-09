using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;
using Umbraco.Core.Logging;
using Umbraco.Core;
using Umbraco.Core.Models;
using Aubergine.Core.Migrations;
using Umbraco.Core.Services;

namespace Aubergine.Auth.Migrations.TargetVersionOne
{
    [AubergineMigration(Authentication.Name, 
        Priorities.Primary + Priorities.ContentType, 
        "{85B5246C-9DB2-4A93-82A0-648A0F22D2BF}")]
    public class AubergineUpdateMemberProperties : AubergineMigrationBase
    {
        public AubergineUpdateMemberProperties(ServiceContext serviceContext, ILogger logger) 
            : base(serviceContext, logger)
        {
        }

        public override void Add()
        {
            var memberTypeService = Services.MemberTypeService;
            var defaultMemberType = memberTypeService.Get("Member");

            var propertyHelper = new Aubergine.Core.Migrations.Helpers.ContentTypeMigrationHelper();

            propertyHelper.AddProperty(defaultMemberType,
                Properties.ResetGuid, "Reset", "Membership",
                "Umbraco.NoEdit",
                "Reset Guid used for account verification and password reset");

            propertyHelper.AddProperty(defaultMemberType,
                Properties.Verified, "Verifed", "Membership",
                "Umbraco.TrueFalse",
                "Has the user verified this account (via email)");

            propertyHelper.AddProperty(defaultMemberType,
                Properties.ExpiryDate, "Expiry", "Membership",
                "Umbraco.DateTime",
                "time at which latest verify or reset request expires");

            memberTypeService.Save(defaultMemberType);
        }

        public override void Remove()
        {
            throw new NotImplementedException();
        }
    }
}
