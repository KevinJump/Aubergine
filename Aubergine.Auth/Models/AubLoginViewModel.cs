using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using UmbracoValidationAttributes;

namespace Aubergine.Auth.Models
{
    public class AubLoginViewModel
    {
        [UmbracoDisplayName("Aub.Auth.Email")]
        [UmbracoRequired("Aub.Auth.Error.MissingEmail")]
        [UmbracoEmail(ErrorMessageDictionaryKey = "Aub.Auth.Error.InvalidEmail")]

        public string EmailAddress { get; set; }

        [UmbracoDisplayName("Aub.Auth.Password")]
        [UmbracoRequired("Aub.Auth.Error.MissingPassword")]
        public string Password { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ReturnUrl { get; set; }
    }
}
