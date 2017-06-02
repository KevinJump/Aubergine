using Aubergine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aubergine.Auth
{
    public class AubergineAuth : IAubergineExtension
    {
        public string Name => "Authentication";
        public string ExtensionId => "{7CD40983-07FC-48BE-B6F9-B9CF3C85DBAB}";
        public string Version => "1.0.0";
    }
}
