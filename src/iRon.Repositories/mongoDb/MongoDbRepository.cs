using iRon.Core.Interfaces;
using iRon.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace iRon.Repositories.mongoDb
{
    public class MongoDbRepository<IEntity> : IiRonRepository<IEntity> where IEntity : IEntity<ObjectId>
    {
        private readonly IMongoCollection<IEntity> collection;
        private readonly IMongoDatabase database;
        readonly IObjectIdCacheRepository<IEntity> cacheRepository;
        public MongoDbRepository(IOptions<MongoDbConfig> mongoConfig, IObjectIdCacheRepository<IEntity> cacheRepository)
        {
            this.cacheRepository = cacheRepository;
            this.database = new MongoClient(mongoConfig.Value.ConnectionString).GetDatabase(mongoConfig.Value.DatabaseName);
            var colname = typeof(IEntity).Name;
            if (colname.EndsWith("Entity", StringComparison.Ordinal))
            {
                colname = colname.Substring(0, colname.Length - 6);
            }

            this.collection = database.GetCollection<IEntity>(colname);
        }

        public async Task<int> CountAsync(Expression<Func<IEntity, bool>> where)
        {
            long ret = await collection.CountDocumentsAsync(where);
            return (int)ret;
        }

        public Task<bool> DeleteAllAsync()
        {
            var cacheName = typeof(IEntity).Name;
            this.collection.DeleteManyAsync(async => 1 == 1);
            this.cacheRepository.DeletePatternAsync("*" + cacheName + "*");
            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(IEntity entity)
        {
            var cacheName = typeof(IEntity).Name;
            this.collection.DeleteOneAsync(d => d.Id == entity.Id);
            this.cacheRepository.DeletePatternAsync("*" + cacheName + "*");
            return Task.FromResult(true);
        }

        public Task<bool> ExistsAsync(Expression<Func<IEntity, bool>> where)
        {
            return this.collection.AsQueryable().AnyAsync();
        }

        public async Task<IEnumerable<IEntity>> FindAsync(Expression<Func<IEntity, bool>> where, string cacheSuffix = "")
        {
            var cacheName = typeof(IEntity).Name + cacheSuffix;

            return await collection.FindAsync(where).Result.ToListAsync();
        }

        public async Task<IEntity> FindFirstAsync(Expression<Func<IEntity, bool>> where, string cacheSuffix = "")
        {
            return await collection.FindAsync(where).Result.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<IEntity>> GetAllAsync()
        {
            var cacheName = typeof(IEntity).Name + "_GetAllAsync";

            IEnumerable<IEntity> ret = await cacheRepository.GetsAsync(cacheName);
            if (ret == null)
            {
                ret = await collection.FindAsync(f => true).Result.ToListAsync();
                cacheRepository.SetAsync(cacheName, ret);
            }
            return ret;

        }

        public Task<IEntity> GetAsync(ObjectId id)
        {
            return collection.FindAsync(f => f.Id == id).Result.SingleAsync();
        }

        public Task<IEntity> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEntity> SaveAsync(IEntity entity)
        {
            if (entity.Id == ObjectId.Empty)
            {
                collection.InsertOneAsync(entity).Wait();
                return Task.FromResult(entity);
            }
            else
            {
                var filter = Builders<IEntity>.Filter.Where(w => w.Id == entity.Id);
                this.collection.ReplaceOneAsync(filter, entity, new UpdateOptions { IsUpsert = true });
                return Task.FromResult(entity);
            }
        }
    }
}
