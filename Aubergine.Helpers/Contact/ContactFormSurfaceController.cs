using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Umbraco.Core;
using Umbraco.Web.Mvc;

namespace Aubergine.Helpers.Contact
{
    public class ContactFormSurfaceController : SurfaceController
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendContactEmail(ContactViewModel model)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            var from = Umbraco.GetDictionaryValue("Contact.From");
            var template =
                Umbraco.ReplaceLineBreaksForHtml(Umbraco.GetDictionaryValue("Contact.Message"));

            var replacements = new Dictionary<string, string>();

            replacements.Add("{{name}}", model.Name);
            replacements.Add("{{email}}", model.Email);
            replacements.Add("{{message}}", model.Message);

            try
            {
                SmtpClient client = new SmtpClient();
                MailMessage message = new MailMessage(from, model.ToAddress);
                message.Body = template.ReplaceMany(replacements);
                message.IsBodyHtml = true;
                client.Send(message);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "Failed to send message : " + ex.Message);
            }

            return RedirectToCurrentUmbracoPage();
        }
    }

    public class ContactViewModel
    {
        public string ToAddress { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
