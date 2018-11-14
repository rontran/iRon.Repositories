
namespace iRon.Repositories.Redis
{
    using iRon.Core.Interfaces;
    using iRon.Repositories.Attributes;
    using iRon.Repositories.Enums;
    using iRon.Repositories.Interfaces;
    using iRon.Repositories.Settings;
    using Microsoft.Extensions.Options;
    using MongoDB.Bson;
    using Newtonsoft.Json;
    using StackExchange.Redis;
    using StackExchange.Redis.Extensions.Core;
    using StackExchange.Redis.Extensions.Newtonsoft;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;

    public class RedisCacheRepository<IEntity> : IObjectIdCacheRepository<IEntity> where IEntity : IEntity<ObjectId>
    {
        private readonly TimeSpan defaultTimeSpan;
        private readonly IRedisCacheConfig cacheConfig;
        private readonly StackExchangeRedisCacheClient client;
        private readonly IConnectionMultiplexer connectionMultiplexer;

        public RedisCacheRepository(IOptions<RedisCacheConfig> cacheConfig)
        {
            this.cacheConfig = cacheConfig.Value;
            this.connectionMultiplexer = ConnectionMultiplexer.Connect(this.cacheConfig.ConnectionString);
            var settings = new JsonSerializerSettings
            {
                // ContractResolver = new CustomResolver()
            };
            settings.Converters.Add(new BsonNullConverter());
            var serializer = new NewtonsoftSerializer(settings);
            if (this.cacheConfig.Enabled)
            {
                this.client = new StackExchangeRedisCacheClient(this.connectionMultiplexer, serializer, 0, this.cacheConfig.Prefix);
            }
            this.defaultTimeSpan = new TimeSpan(0, 0, 0);
        }


        public void DeleteAsync(string key)
        {
            if (client != null && this.cacheConfig.Enabled && IsConnected) this.client.RemoveAsync(key);
        }


        public void DeletePatternAsync(string key)
        {
            if (client != null && this.cacheConfig.Enabled && IsConnected) this.client.RemoveAllAsync(this.client.SearchKeys(key));
        }

        public Task<bool> ExistsAsync(string key)
        {
            if (client != null && this.cacheConfig.Enabled && IsConnected) { return this.client.ExistsAsync(key); }
            else
            {
                return Task.FromResult<bool>(false);
            }
        }

        public void FlushAllAsync()
        {
            if (client != null && this.cacheConfig.Enabled && IsConnected) this.client.FlushDbAsync();
        }

        public Task<IEntity> GetAsync(string key)
        {
            if (client != null && this.cacheConfig.Enabled && IsConnected)
            {
                return this.client.GetAsync<IEntity>(key);
            }
            else
            {
                return Task.FromResult<IEntity>(default(IEntity));
            }
        }

        public Task<IEnumerable<IEntity>> GetsAsync(string key)
        {
            if (client != null && this.cacheConfig.Enabled && IsConnected)
            {
                return this.client.GetAsync<IEnumerable<IEntity>>(key);
            }
            else
            {
                return Task.FromResult<IEnumerable<IEntity>>(default(IEnumerable<IEntity>));
            }
        }

        public void SetAsync(string key, IEntity entity)
        {
            if (client != null && this.cacheConfig.Enabled && IsConnected)
            {
                var attr = typeof(IEntity).GetCustomAttribute<CacheDuration>(false);
                if (attr != null && attr.Duration == Duration.NONE) return;
                var timeSpan = attr == null ? this.defaultTimeSpan : new TimeSpan(0, 0, this.GetDuration(attr.Duration));
                this.client.AddAsync<IEntity>(key, entity, timeSpan);
            }
        }

        public void SetAsync(string key, IEnumerable<IEntity> entity)
        {
            if (client != null && this.cacheConfig.Enabled && IsConnected)
            {
                var attr = typeof(IEntity).GetCustomAttribute<CacheDuration>(false);
                if (attr != null && attr.Duration == Duration.NONE) return;
                var timeSpan = attr == null ? this.defaultTimeSpan : new TimeSpan(0, 0, this.GetDuration(attr.Duration));
                this.client.AddAsync<IEnumerable<IEntity>>(key, entity, timeSpan);
            }
        }

        public bool IsConnected => connectionMultiplexer != null && this.connectionMultiplexer.IsConnected;

        private int GetDuration(Duration duration)
        {
            switch (duration)
            {
                case Duration.NONE:
                    return this.cacheConfig.Duration.None;
                case Duration.LOW:
                    return this.cacheConfig.Duration.Low;
                case Duration.NORMAL:
                    return this.cacheConfig.Duration.Normal;
                case Duration.HIGH:
                    return this.cacheConfig.Duration.High;
                case Duration.FOREVER:
                    return this.cacheConfig.Duration.Forever;
                default:
                    return 0;
            }
        }
    }
}
