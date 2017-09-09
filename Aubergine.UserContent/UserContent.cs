using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aubergine.Core;
using Umbraco.Core.Events;

namespace Aubergine.UserContent
{
    internal class AubergineUserContent : IAubergineExtension
    {
        // //////////////
        public string ExtensionId => "{A7632D11-CA46-43CC-99FC-D02D4259E414}";
        public string Name => UserContent.Name;
        public string Version => UserContent.Version;
        public string ProductName => UserContent.Name;
    }

    public class UserContent
    {
        public const string Name = "AubergineUserContent";
        public const string Version = "1.0.0";
        public const string DefaultInstance = "default";
        public const string TableName = "Aubergine_UserContent";
       

        public static event TypedEventHandler<UserContent, SaveEventArgs<UserContentContext>> Created;

        public static void RaiseContextCreated(UserContentContext context)
        {
            Created.RaiseEvent(new SaveEventArgs<UserContentContext>(context), new UserContent());
        }
    }
}
