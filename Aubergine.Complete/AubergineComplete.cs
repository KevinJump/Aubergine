using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aubergine.Core;
using Semver;
using Umbraco.Core;

namespace Aubergine.Complete
{
    public class AubergineComplete : IAubergineExtension
    {
        public const string InternalName = "Aubergine.Complete";

        public string Name => InternalName;
        public string ExtensionId => "{349F9962-3AE4-4C2B-8DFD-BE56D78C6793}";
        public string Version => "1.0.0";
        public string ProductName => InternalName;
    }


}
