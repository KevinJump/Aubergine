using Aubergine.Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace Aubergine.Auth
{
    public partial class AuthenticationController : SurfaceController
    {

        [ChildActionOnly]
        public ActionResult ForgotPassword()
        {
            return PartialView(Views.ForgotPwd, new AubForgotPasswordModel());
        }

        [HttpPost]
        [NotChildAction]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(AubForgotPasswordModel model)
        {
            TempData["resent"] = false;

            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            var memberService = ApplicationContext.Services.MemberService;

            var member = memberService.GetByEmail(model.EmailAddress);
            if (member != null)
            {
                DateTime expires = DateTime.Now.AddMinutes(30);
                member.SetValue(Properties.ExpiryDate, expires);

                var resetGuid = Guid.NewGuid().ToString();
                member.SetValue(Properties.ResetGuid, resetGuid);
                memberService.Save(member);

                var emailManager = new AuthenticationEmailManager(this.Request);
                emailManager.SendEmail(member.Email,
                    Localize("Aub.PasswordReset.Subject", "Reset your password"),
                    Localize("Aub.PasswordReset.Email", "{url}"),
                    resetGuid,
                    AuthUrls.Reset);

                TempData["true"] = false;
            }
            else
            {
                ModelState.AddModelError(Form.AubAuthKey,
                    Localize("Aub.Auth.Reset.NoUser", "No user found"));
            }

            return CurrentUmbracoPage();
        }
    }
}
