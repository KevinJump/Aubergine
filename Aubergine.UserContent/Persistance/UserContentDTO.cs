using Aubergine.UserContent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Aubergine.UserContent.Persistance
{
    [TableName("UserContent")]
    [PrimaryKey("Id")]
    public class UserContentDTO : IUserContentDTO
    {

        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        public Guid Key { get; set; }
        public string UserContentType { get; set; }

        public string Name { get; set; }

        [NullSetting(NullSetting = NullSettings.Null)]
        public string Url { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }


        [NullSetting(NullSetting = NullSettings.Null)]
        public string Author { get; set; }

        [NullSetting(NullSetting = NullSettings.Null)]
        public string AuthorId { get; set; }

        public int Status { get; set; }

        public Guid? NodeKey { get; set; }

        public Guid? ParentKey { get; set; }

        [SpecialDbType(SpecialDbTypes.NTEXT)]
        public string PropertyData { get; set; }

    }
}
