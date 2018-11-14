using iRon.Core.Enums;
using iRon.Core.Interfaces;
using iRon.Repositories.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iRon.Repositories.Example
{
    [CacheDuration(Enums.Duration.LOW)]
    public class UserEntity : IEntity<ObjectId>
    {
        public ObjectId Id { get ; set ; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Tag { get ; set ; }
        public DateTimeOffset? CreatedDate { get ; set ; }
        public DateTimeOffset? UpdatedDate { get ; set ; }
        public ObjectId UpdatedById { get ; set ; }
        public ObjectId CreatedById { get ; set ; }
        public bool isNew { get; }
        public bool isDirty { get; }
        public string LegacyId { get ; set ; }
        public ObjectStatusEnum ObjectStatus { get ; set ; }
    }
}
