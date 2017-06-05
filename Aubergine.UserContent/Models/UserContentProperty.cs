using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace Aubergine.UserContent
{
    /// <summary>
    ///  a simple property element for user content, 
    ///  can get or set - will be written back on save
    /// </summary>
    public class UserContentProperty 
    {
        public UserContentProperty() { }

        public UserContentProperty(string alias, object value)
        {
            PropertyAlias = alias;
            Value = value;
        }

        public string PropertyAlias { get; set; }

        private object _value;

        public bool HasValue
        {
            get
            {
                return _value != null;
            }
        }

        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}
