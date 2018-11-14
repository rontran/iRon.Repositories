using iRon.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace iRon.Repositories.mongoDb
{
    public class MongoDbUnitOfWork : IUnitOfWork
    {
        public void BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public void CommitTransaction()
        {
            throw new NotImplementedException();
        }

        public bool IsInTransaction()
        {
            throw new NotImplementedException();
        }

        public void RollBackTransaction()
        {
            throw new NotImplementedException();
        }
    }
}
