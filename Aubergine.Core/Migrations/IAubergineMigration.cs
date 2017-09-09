using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aubergine.Core.Migrations
{
    /// <summary>
    ///  our own migration interface.
    /// </summary>
    public interface IAubergineMigration
    {
        void Add();
        void Remove();
    }
}
