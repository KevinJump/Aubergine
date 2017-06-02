using Aubergine.Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace Aubergine.Auth
{
    public partial class AuthenticationController : SurfaceController
    {
        [ChildActionOnly]
        public ActionResult Login()
        {
            AubLoginViewModel loginModel = new AubLoginViewModel();
            loginModel.ReturnUrl = HttpContext.Request.Url.ToString();

            if (HttpContext.Request.Url.AbsolutePath == AuthUrls.Login)
                loginModel.ReturnUrl = "/";

            return PartialView(Views.Login, loginModel);
        }

        [HttpPost]
        [NotChildAction]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AubLoginViewModel loginModel)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            if (Umbraco.MembershipHelper.IsLoggedIn())
                return RedirectToRoute(loginModel.ReturnUrl);

            try
            {
                if (Umbraco.MembershipHelper.Login(loginModel.EmailAddress, loginModel.Password))
                {
                    // logged in.
                    var member = Umbraco.MembershipHelper.GetByEmail(loginModel.EmailAddress);
                    if (member != null)
                    {
                        if (member.GetPropertyValue<bool>(Properties.Verified))
                        {
                            // approved.
                            TempData["returnUrl"] = loginModel.ReturnUrl;
                            return RedirectToCurrentUmbracoPage();
                        }
                        else
                        {
                            ModelState.AddModelError(Form.AubAuthKey,
                                Localize("Aub.Auth.Login.NoValid", "Account not approved"));

                            Umbraco.MembershipHelper.Logout();
                        }
                    }
                    else
                    {
                        // cannot find the user 
                        ModelState.AddModelError(Form.AubAuthKey,
                            Localize("Aub.Auth.Login.NoUser", "No User"));
                    }
                }
                else
                {
                    ModelState.AddModelError(Form.AubAuthKey,
                        Localize("Aub.Auth.Login.Fail", "Login Failed"));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(Form.AubAuthKey,
                    Localize("Aub.Auth.Fail", "Login Failed"));
            }

            return CurrentUmbracoPage();
        }

        public ActionResult Logout(string returnUrl)
        {
            if (Umbraco.MembershipHelper.IsLoggedIn())
            {
                Umbraco.MembershipHelper.Logout();
            }

            return Redirect(returnUrl);
        }

    }
}
