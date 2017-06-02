using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmbracoValidationAttributes;

namespace Aubergine.Auth.Models
{
    public class AubRegisterViewModel
    {
        [UmbracoDisplayName("Aub.Auth.Name")]
        [UmbracoRequired("Aub.Auth.Error.MissingName")]
        public string Name { get; set; }

        [UmbracoDisplayName("Aub.Auth.Email")]
        [UmbracoRequired("Aub.Auth.Error.MissingEmail")]
        [UmbracoEmail(ErrorMessageDictionaryKey = "Aub.Auth.Error.InvalidEmail")]
        public string EmailAddress { get; set; }

        [UmbracoDisplayName("Aub.Auth.Password")]
        [UmbracoRequired("Aub.Auth.Error.MissingPassword")]
        public string Password { get; set; }

        [UmbracoDisplayName("Aub.Auth.PasswordConfirm")]
        [UmbracoRequired("Aub.Auth.Error.PasswordMismatch")]
        public string PasswordConfirm { get; set; }
    }
}
