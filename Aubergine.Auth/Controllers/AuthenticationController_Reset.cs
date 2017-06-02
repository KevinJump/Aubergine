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
        public ActionResult ResetPassword()
        {
            return PartialView(Views.Reset, new AubPasswordResetModel());
        }

        [HttpPost]
        [NotChildAction]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(AubPasswordResetModel model)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            var memberService = ApplicationContext.Services.MemberService;
            try
            {
                var member = memberService.GetByEmail(model.EmailAddress);
                if (member != null)
                {
                    var resetGuid = Request.QueryString["resetGuid"];

                    if (member.GetValue<string>(Properties.ResetGuid) == resetGuid)
                    {
                        var expires = member.GetValue<DateTime>(Properties.ExpiryDate);
                        if (DateTime.Now.CompareTo(expires) < 0)
                        {
                            // we can reset 
                            memberService.SavePassword(member, model.Password);
                            member.SetValue(Properties.ResetGuid, string.Empty);
                            member.SetValue(Properties.ExpiryDate, DateTime.MinValue);
                            memberService.Save(member);
                        }
                        else
                        {
                            ModelState.AddModelError(Form.AubAuthKey,
                                Localize("Aub.Auth.Reset.Expired", "Request Expired"));
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(Form.AubAuthKey,
                            Localize("Aub.Auth.Reset.NoRequest", "No reset request in system"));
                    }
                }
                else
                {
                    ModelState.AddModelError(Form.AubAuthKey,
                        Localize("Aub.Auth.Reset.NoAccount", "We cannot find the account"));
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(Form.AubAuthKey,
                    Localize("Aub.Auth.Reset.Error", "An error occured resetting your account"));
            }

            return CurrentUmbracoPage();
        }
    }
}
