﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Aubergine.UserContent.Persistance.Models
{
    [TableName(UserContent.TableName)]
    [PrimaryKey("Id")]
    public class UserContentDTO
    {
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        // [Index(IndexTypes.UniqueNonClustered, Name = "IX_UserContentKeys")]
        public Guid Key { get; set; }

        public string UserContentType { get; set; }

        public string Name { get; set; }

        [NullSetting(NullSetting = NullSettings.Null)]
        public string Url { get; set; }

        public string Author { get; set; }
        public string AuthorId { get; set; }
        
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public int Status { get; set; }

        public Guid? NodeKey { get; set; }
        public Guid? ParentKey { get; set; }
        
        [SpecialDbType(SpecialDbTypes.NTEXT)]
        public string PropertyData { get; set; }
    }
}
