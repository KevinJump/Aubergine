using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Aubergine.Core.Migrations
{
    public class AubergineMigrationService
    {
        DatabaseContext _dbContext;

        public AubergineMigrationService(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        public IEnumerable<AubergineConfig> GetAll()
        {
            var sql = new Sql()
                .Select("*")
                .From("Aubergine_Configuration");

            return _dbContext.Database.Fetch<AubergineConfig>(sql);
        }

        public void Add(string name, string key)
        {
            using (var db = _dbContext.Database)
            {
                var item = new AubergineConfig
                {
                    Name = name,
                    Key = key
                };

                using (Transaction transaction = db.GetTransaction())
                {
                    db.Save(item);
                    transaction.Complete();
                }
            }
        }
    }

    [TableName("Aubergine_Configuration")]
    [PrimaryKey("Id")]
    public class AubergineConfig
    {
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Key { get; set; }
    }
}
