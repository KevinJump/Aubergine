using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmbracoValidationAttributes;

namespace Aubergine.Auth.Models
{
    public class AubForgotPasswordModel
    {
        [UmbracoDisplayName("Aub.Auth.Email")]
        [UmbracoRequired("Aub.Auth.Error.MissingEmail")]
        [UmbracoEmail(ErrorMessageDictionaryKey = "Aub.Auth.Error.InvalidEmail")]
        public string EmailAddress { get; set; }
    }

    public class AubPasswordResetModel
    {
        [UmbracoDisplayName("Aub.Auth.Email")]
        [UmbracoRequired("Aub.Auth.Error.MissingEmail")]
        [UmbracoEmail(ErrorMessageDictionaryKey = "Aub.Auth.Error.InvalidEmail")]
        public string EmailAddress { get; set; }

        [UmbracoDisplayName("Aub.Auth.Password")]
        [UmbracoRequired("Aub.Auth.Error.MissingPassword")]
        public string Password { get; set; }

        [UmbracoDisplayName("Aub.Auth.PasswordConfirm")]
        [UmbracoRequired("Aub.Auth.Error.PasswordMismatch")]
        public string ConfimPassword { get; set; }
    }
}
