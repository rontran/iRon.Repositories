using iRon.Core.Interfaces;
using MongoDB.Bson;

namespace iRon.Repositories.Interfaces
{
    public interface IObjectIdCacheRepository<T> :  ICacheRepository<T, ObjectId> where T:IEntity<ObjectId>
    {
    }
}
