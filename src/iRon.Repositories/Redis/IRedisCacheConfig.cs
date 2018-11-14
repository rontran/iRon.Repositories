using System;
using System.Collections.Generic;
using System.Text;

namespace iRon.Repositories.Redis
{
    public interface IRedisCacheConfig {
        bool Enabled { get; set; }
        DurationConfig Duration { get; set; }
        string Prefix { get; set; }
        string ConnectionString { get; set; }
    }
    public class RedisCacheConfig: IRedisCacheConfig
    {
        public string ConnectionString { get; set; }
        public string Prefix { get; set; }
        public bool Enabled { get; set; }
        public DurationConfig Duration { get; set; } = new DurationConfig();
    }

    public class DurationConfig
    {
        public int None { get; set; } = 0;
        public int Low { get; set; } = 30;
        public int Normal { get; set; } = 300;
        public int High { get; set; } = 3600;
        public int Forever { get; set; } = 86400;
    }



}
