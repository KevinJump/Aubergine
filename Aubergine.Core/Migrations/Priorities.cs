using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aubergine.Core.Migrations
{
    public class Priorities
    {
        // order of the whole item, 
        public const int Core = 0;
        // so things that are required by other things are Primary
        public const int Primary = 1000;

        // the packages at the end are Standard
        public const int Standard = 2000;

        // for things that really require everything (i.e complete)
        public const int Late = 9000;


        // within an item, you should obey these orders to 
        // make sure things are installed rightly.
        public const int Language = 0;
        public const int Dictionary = 1;

        public const int DataType = 100;
        public const int Template = 200;
        public const int ContentType = 300;
        public const int MediaType = 400;
        public const int Media = 800;
        public const int Content = 900;
    }
}
