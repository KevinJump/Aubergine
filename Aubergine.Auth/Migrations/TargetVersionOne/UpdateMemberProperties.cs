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

namespace Aubergine.Auth.Migrations.TargetVersionOne
{

    [Migration("1.0.0", 0, Product.Name)]
    public class UpdateMemberProperties : MigrationBase
    {
        public UpdateMemberProperties(ISqlSyntaxProvider sqlSyntax, ILogger logger)
            : base(sqlSyntax, logger)
        {
        }

        public override void Up()
        {
            var memberTypeService = ApplicationContext.Current.Services.MemberTypeService;
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

        public override void Down()
        { }

    }
}
