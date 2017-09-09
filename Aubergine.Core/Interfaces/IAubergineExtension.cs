using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aubergine.Core
{
    public interface IAubergineExtension
    {
        string Name { get; }
        string ExtensionId { get; }
        string Version { get; }
        string ProductName { get; }
    }
}
