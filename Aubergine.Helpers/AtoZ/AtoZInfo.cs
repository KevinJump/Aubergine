using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aubergine.Helpers
{
    /// <summary>
    ///  AtoZ info model, stored in cache
    ///  for each page in the atoz.
    /// </summary>
    public class AtoZInfo
    {
        public string Title { get; set; }
        public int Id { get; set; }
        public string Url { get; set; }
    }
}
