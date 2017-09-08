using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aubergine.UserContent.Cache
{
    public class UserContentKeyItems
    { 
        public UserContentKeyItems()
        {
            ItemKeys = new List<Guid>();
        }

        public Guid Key { get; set; }
        public List<Guid> ItemKeys { get; set; } 
    }

    public class UserContentItemCount
    {
        public int Count { get; set; }
    }
}
