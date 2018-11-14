using iRon.Core.Interfaces;
using MongoDB.Bson;
using System;

namespace iRon.Repositories.Interfaces
{
    public interface IiRonRepository<IEntity> : IRepository<IEntity, ObjectId> where IEntity : IEntity<ObjectId>
    { }
}
