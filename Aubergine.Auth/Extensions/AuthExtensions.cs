using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Web;
using Umbraco.Web.PublishedCache;

namespace Aubergine.Auth
{
    public static class AuthExtensions
    {
        public static string GetUserImage(this MemberPublishedContent member)
        {
            var md5 = MD5.Create();

            // at the moment this only works when ModelsBuilder is off, because it 
            // doesn't let you get to MemberPublishedContent from the GetCurrentMember call :(
            //

            if (member != null)
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(member.Email.Trim().ToLower());
                var hash = BitConverter.ToString(md5.ComputeHash(inputBytes)).Replace("-", "").ToLower();
                return "https://www.gravatar.com/avatar/" + hash;
            }
            return "https://www.gravatar.com/avatar/?d=mm";
        }
    }
}
