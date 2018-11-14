using iRon.Core.Interfaces;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace iRon.Repositories.Interfaces
{
    public interface IObjectIdEntity:IEntity<ObjectId>
    {
    }
}
