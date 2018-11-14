using iRon.Repositories.Interfaces;
using iRon.Repositories.mongoDb;
using iRon.Repositories.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using iRon.Core.Interfaces;
using MongoDB.Bson;

namespace iRon.Repositories
{
    public static class Startup
    {
        public static IServiceCollection UseRepository(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<MongoDbConfig>(config.GetSection("Database"));
            services.AddSingleton(typeof(IiRonRepository<>), typeof(MongoDbRepository<>));


            //Redis
            services.Configure<RedisCacheConfig>(config.GetSection("iRon.Cache"));
            services.AddSingleton(typeof(IObjectIdCacheRepository<>), typeof(RedisCacheRepository<>));
            

            return services;




        }
    }
}
