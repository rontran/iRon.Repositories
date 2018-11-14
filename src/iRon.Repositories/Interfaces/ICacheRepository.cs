using iRon.Core.Interfaces;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iRon.Repositories.Interfaces
{
    public interface ICacheRepository<IEntity,IdType> where IdType:struct where IEntity : IEntity<ObjectId>
    {
        void SetAsync(string key, IEnumerable<IEntity> entity);
        void SetAsync(string key, IEntity entity);
        bool IsConnected { get; }
        Task<IEntity> GetAsync(string key);
        Task<IEnumerable<IEntity>> GetsAsync(string key);
        void FlushAllAsync();
        Task<bool> ExistsAsync(string key);
        void DeleteAsync(string key);
        void DeletePatternAsync(string key);

    }
}
