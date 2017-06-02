using Aubergine.Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Umbraco.Web;
using Umbraco.Web.Security;
using Umbraco.Web.Mvc;

namespace Aubergine.Auth
{
    /// <summary>
    ///  Partial class
    ///  Authentication Controller, placeholder for 
    ///  all authentication things.
    /// </summary>
    [PluginController("Aubergine")]
    public partial class AuthenticationController : SurfaceController
    {

        public JsonResult CheckForValidEmail(string emailAddress)
        {
            if (IsValidEmailDomain(emailAddress))
                return Json(true, JsonRequestBehavior.AllowGet);
            else
                return Json(Localize("Aub.Auth.Email.Blocked", "Invalid Email address for this site"));
        }

        public JsonResult CheckForEmailAddress(string emailAddress)
        {
            if (EmailAddressExists(emailAddress))
                return Json(Localize("Aub.Auth.Email.InUse", "email already in use"));
            else
                return Json(true, JsonRequestBehavior.AllowGet);
        }


        private string Localize(string key, string defaultValue)
        {
            var dictionaryValue = Umbraco.GetDictionaryValue(key);
            if (string.IsNullOrEmpty(dictionaryValue))
                return defaultValue;

            return dictionaryValue;
        }
        private bool EmailAddressExists(string email)
        {
            return Members.GetByEmail(email) != null;
        }

        private bool IsValidEmailDomain(string emailAddress)
        {
            var whitelist = CurrentPage.GetPropertyValue<string>("domainWhiteList", "").ToLower();
            var blacklist = CurrentPage.GetPropertyValue<string>("domainBlackList", "").ToLower();

            if (emailAddress.Contains("@"))
            {
                var domain = emailAddress.Substring(emailAddress.IndexOf('@')).ToLower();

                if (!string.IsNullOrWhiteSpace(whitelist) && !whitelist.Contains(domain))
                    return false;

                if (!string.IsNullOrWhiteSpace(blacklist) && blacklist.Contains(domain))
                    return false;

                return true;
            }

            return false; 
        }

    }
}
