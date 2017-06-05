using Aubergine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aubergine.Complete
{
    /// <summary>
    ///  Aubergine complete, just a placeholder for the full nuget package that 
    ///  will install the whole shabang in one . 
    /// </summary>
    public class AubergineComplete : IAubergineExtension
    {
        public string Name => "Aubergine Complete";
        public string ExtensionId => "{349F9962-3AE4-4C2B-8DFD-BE56D78C6793}";
        public string Version => "1.0.0";
    }
}
