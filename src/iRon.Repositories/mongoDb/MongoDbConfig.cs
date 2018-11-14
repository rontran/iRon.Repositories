using System;
using System.Collections.Generic;
using System.Text;

namespace iRon.Repositories.mongoDb
{
    public interface IMongoDbConfig {
        string UserName { get; set; }
        string Password { get; set; }
        string DatabaseName { get; set; }
        string ConnectionString { get; set; }
    }

    public class MongoDbConfig: IMongoDbConfig
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
