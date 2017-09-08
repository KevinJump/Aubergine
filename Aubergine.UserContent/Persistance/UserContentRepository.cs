using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aubergine.UserContent.Models;
using Aubergine.UserContent.Persistance.Models;
using AutoMapper;
using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace Aubergine.UserContent.Persistance
{
    public class UserContentRepository<TUserContent, TUserContentDTO> : IUserContentRepository
        where TUserContentDTO : UserContentDTO 
        where TUserContent : IUserContent
    {
        private readonly DatabaseContext _dbContext;
        private readonly string _tableName;

        public UserContentRepository(DatabaseContext dbContext)
        {
            _tableName = "Aubergine_UserContent";
            _dbContext = dbContext;
        }

        public UserContentRepository(DatabaseContext dbContext, string tableName)
        {
            _tableName = tableName;
            _dbContext = dbContext;
        }

        private Database getDb()
        {
            return _dbContext.Database;
        }

        private Sql getBaseSql()
        {
            return new Sql()
                    .Select("*")
                    .From(_tableName);
        }

        public IUserContent Get(int id)
        {
            using (var db = getDb())
            {
                var sql = getBaseSql()
                    .Where("[Id] = @0", id);

                var item = db.SingleOrDefault<TUserContentDTO>(sql);
                if (item != null)
                    return Mapper.Map<TUserContent>(item);

                return default(TUserContent);
            }
        }

        ////// gettters
        public IUserContent Get(Guid key)
        {
            using (var db = getDb())
            {
                var sql = getBaseSql()
                    .Where("[Key] = @0", key);

                var item = db.SingleOrDefault<TUserContentDTO>(sql);
                if (item != null)
                    return Mapper.Map<TUserContent>(item);

                return default(TUserContent);
            }
        }

        public IEnumerable<IUserContent> GetByContentKey(
            Guid contentKey, UserContentStatus status = UserContentStatus.Approved,
            bool getAll = false,
            string contentType = "")
        {
            using(var db = getDb())
            {
                var sql = getBaseSql()
                    .Where("[NodeKey] = @0", contentKey);

                if (!contentType.IsNullOrWhiteSpace())
                    sql.Where("[UserContentType] = @0", contentType);

                if (!getAll)
                    sql.Where("[Status] = @0", (int)status);

                var results = db.Fetch<TUserContentDTO>(sql)
                    .Select(x => Mapper.Map<TUserContent>(x));

                return (IEnumerable<IUserContent>)results;
            }
        }

        public int GetContentCount(Guid key)
        {
            using(var db = getDb())
            {
                var sql = new Sql()
                    .Select("COUNT(*)")
                    .From(_tableName)
                    .Where("[NodeKey] = @0", key);

                return db.ExecuteScalar<int>(sql);
            }
        }

        public int GetChildCount(Guid key)
        {
            using (var db = getDb())
            {
                var sql = new Sql()
                    .Select("COUNT(*)")
                    .From(_tableName)
                    .Where("[ParentKey] = @0", key);

                return db.ExecuteScalar<int>(sql);
            }
        }

        public IEnumerable<IUserContent> GetChildren(
            Guid key, UserContentStatus status = UserContentStatus.Approved,
            bool getAll = false, string contentType = "")
        {
            using(var db = getDb())
            {
                var sql = getBaseSql()
                    .Where("[ParentKey] = @0", key);

                if (!contentType.IsNullOrWhiteSpace())
                    sql.Where("[UserContentType = @0", contentType);

                if (!getAll)
                    sql.Where("Status = @0", (int)status);

                return db.Fetch<TUserContentDTO>(sql)
                    .Select(x => (IUserContent)Mapper.Map<TUserContent>(x));
            }
        }

        public IUserContent Create(IUserContent entity)
        {
            using(var db = getDb())
            {
                var dto = Mapper.Map<TUserContentDTO>((TUserContent)entity);
                if (dto.UserContentType.IsNullOrWhiteSpace())
                    dto.UserContentType = "default";

                using (Transaction transaction = db.GetTransaction())
                {
                    db.Save(_tableName, "Id", dto);
                    transaction.Complete();
                }

                return Mapper.Map<TUserContent>(dto);
            }
        }

        public IUserContent Update(IUserContent entity)
        {
            using (var db = getDb())
            {
                var dto = Mapper.Map<TUserContentDTO>((TUserContent)entity);
                using (Transaction transaction = db.GetTransaction())
                {
                    db.Update(_tableName, "Id", dto);
                    transaction.Complete();
                }

                return Mapper.Map<TUserContent>(dto);
            }
        }

        public void Delete(Guid key)
        {
            using(var db = getDb())
            {
                using(Transaction transaction = db.GetTransaction())
                {
                    var sql = getBaseSql()
                        .Where("[Key] = @0", key);

                    db.Delete<TUserContentDTO>(sql);
                    transaction.Complete();
                }
            }
        }

        public int SetStatus(Guid key, UserContentStatus status)
        {
            using(var db = getDb())
            {
                using (Transaction transaction = db.GetTransaction())
                {
                    var statement = $"UPDATE [{_tableName}] SET STATUS = @status " +
                        $"WHERE [Key] = @key";

                    var rows = db.Execute(statement, new { status = (int)status, key = key });
                    transaction.Complete();

                    return rows;
                }
            }
        }
    }
}
