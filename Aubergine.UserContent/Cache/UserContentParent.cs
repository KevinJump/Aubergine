using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aubergine.UserContent.Cache
{
    /// <summary>
    ///  Key Value pair , for node and User Content Keys - so we can store
    ///  lists in cache if needed.
    /// </summary>
    public class UserContentParent
    {
        public Guid NodeKey { get; set; }
        public List<Guid> Keys { get; set; }
    }
}
