using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aubergine.Core.Migrations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AubergineMigrationAttribute : Attribute
    {
        public string ItemName { get;set;}
        public int SortOrder { get; set; }
        public string Key { get; set; }


        public AubergineMigrationAttribute(string name, int sortOrder, string key)
        {
            ItemName = name;
            SortOrder = sortOrder;
            Key = key;
        }
    }
}
