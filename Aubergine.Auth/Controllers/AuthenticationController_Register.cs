using Aubergine.Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using Umbraco.Core.Logging;

namespace Aubergine.Auth
{
    public partial class AuthenticationController: SurfaceController 
    {
        [ChildActionOnly]
        public ActionResult Register()
        {
            return PartialView(Views.Register,
                new AubRegisterViewModel());
        }

        [HttpPost]
        [NotChildAction]
        [ValidateAntiForgeryToken]
        public ActionResult Register(AubRegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            var memberService = ApplicationContext.Services.MemberService;
            if (EmailAddressExists(model.EmailAddress))
            {
                ModelState.AddModelError(Form.AubAuthKey,
                    Localize("Aub.Auth.Register.ExistingEmail", "Email address already registered"));
                return CurrentUmbracoPage();
            }

            var memberTypeService = ApplicationContext.Services.MemberTypeService;
            var memberType = memberTypeService.Get("Member");

            try
            {
                var member = memberService.CreateMemberWithIdentity(
                    model.EmailAddress, model.EmailAddress, model.Name, memberType);

                memberService.SavePassword(member, model.Password);

                if (member.HasProperty(Properties.Verified))
                    member.SetValue(Properties.Verified, false);

                if (member.HasProperty(Properties.ExpiryDate))
                    member.SetValue(Properties.ExpiryDate, DateTime.Now.AddDays(1));

                member.SetValue("umbracoMemberComments",
                    string.Format("{0} {1}", model.EmailAddress, model.Password));

                member.IsApproved = false;
                memberService.Save(member);

                var emailManager = new AuthenticationEmailManager(this.Request);
                emailManager.SendEmail(member.Email,
                    Localize("Aub.Auth.Verify.Subject", "Verify your account"),
                    Localize("Aub.Auth.Verify.Email", "{url}"),
                    member.Key.ToString(),
                    AuthUrls.Verify);
            }
            catch(Exception ex)
            {
                Logger.Warn<AuthenticationController>("Error while registering {0}", ()=> ex.ToString());

                ModelState.AddModelError(Form.AubAuthKey,
                    Localize("Aub.Auth.Register.Error", "An error occured"));
            }

            return CurrentUmbracoPage();
        }

        public ActionResult Verify(string guid)
        {

            Guid memberKey;
            if (Guid.TryParse(guid, out memberKey))
            {
                var memberService = ApplicationContext.Services.MemberService;

                var member = memberService.GetByKey(memberKey);
                if (member != null)
                {
                    member.SetValue(Properties.Verified, true);
                    member.IsApproved = true;

                    memberService.Save(member);
                    return Content(Localize("Aub.Auth.Verified", "Account Verified"));
                }
            }

            return Content(Localize("Aub.Auth.Verify.NoAccount", "No Account Found"));
        }
    }
}
