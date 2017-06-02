using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Configuration;

namespace Aubergine.Auth
{
    public class AuthenticationEmailManager
    {
        private readonly HttpRequestBase request; 

        public AuthenticationEmailManager(HttpRequestBase requestBase)
        {
            request = requestBase;
        }

        public void SendEmail(string to, string subject, string format, string resetGuid, string url)
        {
            try
            {
                string fromAddress = UmbracoConfig.For.UmbracoSettings().Content
                    .NotificationEmailAddress;

                MailMessage mailMessage = new MailMessage(fromAddress, to);
                mailMessage.Subject = subject;

                string baseUrl = request.Url.AbsoluteUri
                    .Replace(request.Url.AbsolutePath, string.Empty);

                var resetUrl = baseUrl + url + "?resetGuid=" + resetGuid;

                Dictionary<string, string> replacements = new Dictionary<string, string>();
                replacements.Add("{url}", resetUrl);

                mailMessage.Body = format.ReplaceMany(replacements);
                mailMessage.IsBodyHtml = true;

                SmtpClient mailClient = new SmtpClient();
                mailClient.Send(mailMessage);
            }
            catch(Exception ex)
            {
                // doh/ 
            }
        }
    }
}
