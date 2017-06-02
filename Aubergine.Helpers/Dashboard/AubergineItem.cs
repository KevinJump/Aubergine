using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aubergine.Helpers.Dashboard
{
    public class AubergineItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Depends { get; set; }
        public string Nuget { get; set; }
        public string Package { get; set; }
    }
}
