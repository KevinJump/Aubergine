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

            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(member.Email.Trim().ToLower());
            var hash = BitConverter.ToString(md5.ComputeHash(inputBytes)).Replace("-", "").ToLower();
            return "https://www.gravatar.com/avatar/" + hash;
        }
    }
}
