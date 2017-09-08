using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Aubergine.Forums.Models
{
    [TableName(AubergineForums.Table)]
    [PrimaryKey("id")]
    public class ForumPostsDTO
        : Aubergine.UserContent.Persistance.Models.UserContentDTO
    {
        public int Level { get; set; }

        public bool Answer { get; set; }

        [NullSetting(NullSetting = NullSettings.Null)]
        public int UpVotes { get; set; }

        [NullSetting(NullSetting = NullSettings.Null)]
        public int DownVotes { get; set; }

    }
}
