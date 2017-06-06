using Aubergine.UserContent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Persistence;
using AutoMapper;
using Umbraco.Core.Persistence.Querying;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.Migrations.Syntax.Create;

namespace Aubergine.UserContent.Persistance
{
    /// <summary>
    ///  Default reposistory for user content, puts 
    ///  everything into a UserComments table, and 
    ///  serializes properties into / from xml
    ///  <para>
    ///     This repo doesn't use linq for building the queries (anymore)
    ///     because we can't then use it for generic tables, as we might
    ///     want. 
    ///  </para>
    /// </summary>
    public class UserContentRepository : IUserContentRepository
    {
        private readonly DatabaseContext _dbContext;
        private readonly string _tableName;

        public UserContentRepository(DatabaseContext dbContext)
        {
            _tableName = "UserContent";
            _dbContext = dbContext; 
        }

        public UserContentRepository(DatabaseContext dbContext, string tableName)
        {
            _tableName = tableName;
            _dbContext = dbContext;
        }

        public IUserContent Get(Guid key)
        {
            using (var db = GetDb())
            {
                var sql = new Sql()
                        .Select("*")
                        .From(_tableName)
                        .Where("[Key] = @0", key);

                var dbContent = db.SingleOrDefault<UserContentDTO>(sql);
                if (dbContent != null )
                    return Mapper.Map<UserContentItem>(dbContent);

                return null;
            }
        }

        public IUserContent Get(int id)
        {
            using(var db = GetDb())
            {
                var sql = new Sql()
                    .Select("*")
                    .From(_tableName)
                    .Where("[id] = @0", id);

                var dbContent = db.SingleOrDefault<UserContentDTO>(sql);
                if (dbContent != null)
                    return Mapper.Map<UserContentItem>(dbContent);

                return null;
            }
        }

        public IEnumerable<IUserContent> GetByContentId(Guid contentKey, bool getAll = false)
        {
            return GetByContentId(contentKey, UserContentStatus.Approved, getAll);
        }

        public IEnumerable<IUserContent> GetByContentId(Guid contentKey,
            UserContentStatus status = UserContentStatus.Approved,
            bool getAll = false,
            string contentType = ""
            )
        {
            using (var db = GetDb())
            {
                var sql = new Sql()
                    .Select("*")
                    .From(_tableName)
                    .Where("NodeKey = @0", contentKey);

                if (!string.IsNullOrWhiteSpace(contentType))
                    sql.Where("UserContentType = '@0'", contentType);

                // only get things of the right status. 
                if (!getAll)
                    sql.Where("STATUS = @0", (int)status);

                var results = db.Fetch<UserContentDTO>(sql);

                return Mapper.Map<IEnumerable<UserContentItem>>(results);
            }
        }

        public IEnumerable<IUserContent> GetChildren(Guid key, bool getAll = false)
        {
            return GetChildren(key, UserContentStatus.Approved, getAll);
        }

        public IEnumerable<IUserContent> GetChildren(
            Guid key, 
            UserContentStatus status = UserContentStatus.Approved, 
            bool getAll = false,
            string contentType = "")
        {
            using (var db = GetDb())
            {
                var sql = new Sql()
                    .Select("*")
                    .From(_tableName)
                    .Where("[ParentKey] = @0", key);

                if (!contentType.IsNullOrWhiteSpace())
                    sql.Where("UserContentType = '@0'", contentType);

                if (!getAll)
                    sql.Where("Status = @0", (int)status);

                var results = db.Fetch<UserContentDTO>(sql);
                return Mapper.Map<IEnumerable<UserContentItem>>(results);
            }
        }

        public IUserContent Create(IUserContent entity)
        {
            using (var db = GetDb())
            {
                var dto = Mapper.Map<UserContentDTO>(entity);
                if (dto.UserContentType.IsNullOrWhiteSpace())
                    dto.UserContentType = "default";

                using (Transaction transaction = db.GetTransaction())
                {
                    db.Save(_tableName, "Id", dto);
                    transaction.Complete();
                }

                return Mapper.Map<UserContentItem>(dto);
            }
        }

        public IUserContent Update(IUserContent entity)
        {
            using (var db = GetDb())
            {
                var dto = Mapper.Map<UserContentDTO>(entity);
                using (Transaction transaction = db.GetTransaction())
                {
                    db.Update(_tableName, "Id", dto);
                    transaction.Complete();
                }
                return Mapper.Map<UserContentItem>(dto);
            }
        }

        public void Delete(Guid key)
        {
            using (var db = GetDb())
            {
                using (Transaction transaction = db.GetTransaction())
                {
                    var sql = new Sql()
                        .Select("*")
                        .From(_tableName)
                        .Where("[Key] = @0", key);
                        // .Where<UserContentDTO>(x => x.Key == key, _dbContext.SqlSyntax);

                    db.Delete<UserContentDTO>(sql);
                    transaction.Complete();
                }
            }
        }

        public bool SetStatus(Guid key, UserContentStatus status)
        {
            LogHelper.Info<IUserContentRepository>("Update: {0}", () => key);

            using (var db = GetDb())
            {
                using (Transaction transaction = db.GetTransaction())
                {
                    var sql = new Sql()
                        .Select("*")
                        .From(_tableName)
                        .Where("[Key] = @0", key);
                        // .Where<UserContentDTO>(x => x.Key == key, _dbContext.SqlSyntax);

                    var item = db.Single<UserContentDTO>(sql);

                    if (item != null)
                    {
                        LogHelper.Info<IUserContentRepository>("Updating and saving to db");
                        item.Status = (int)status;
                        db.Update(_tableName, "Id", item);
                        transaction.Complete();
                    }
                    return true;
                }
            }
        }



        private Database GetDb()
        {
            return _dbContext.Database;
        }
    }
}
