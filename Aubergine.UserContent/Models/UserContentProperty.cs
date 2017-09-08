namespace Aubergine.UserContent.Models
{
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
        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public bool HasValue
        {
            get { return _value != null; }
        }
    }
}
