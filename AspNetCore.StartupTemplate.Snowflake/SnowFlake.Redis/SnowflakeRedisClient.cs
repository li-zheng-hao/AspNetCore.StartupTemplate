using System.Collections.Concurrent;
using AspNetCore.StartUpTemplate.Configuration;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace AspNetCore.StartupTemplate.Snowflake.SnowFlake.Redis
{
    public class SnowflakeRedisClient : ISnowflakeRedisClient
    {
        private readonly string _instance;
        private readonly RedisOption _options;
        private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(initialCount: 1, maxCount: 1);
        private ConnectionMultiplexer _conn { get; set; }
        /// <summary>
        /// 注意，这里连接的是哨兵，每次都需要获取master
        /// </summary>
        /// <param name="options"></param>
        /// <param name="conn"></param>
        public SnowflakeRedisClient( IOptions<RedisOption> options,IConnectionMultiplexer conn)
        {
            _options = options.Value;
            _instance = _options.InstanceName;
            _conn =(ConnectionMultiplexer) conn;

        }

        private IDatabase Connect(int db = 0, CancellationToken token = default)
        {
            db = db < 0 ? _options.Database : db;
           
            var masterConfig = new ConfigurationOptions
            {
                CommandMap = CommandMap.Default,
                ServiceName = AppSettingsConstVars.RedisServiceName,
                Password = AppSettingsConstVars.RedisPassword
            };
            var _masterConnectionconn = _conn.GetSentinelMasterConnection(masterConfig, Console.Out);
            var _db=_masterConnectionconn.GetDatabase();
            return _db;
        }



        public void Dispose()
        {
            _connectionLock?.Dispose();
            if (_conn != null)
            {
                _conn.Close();
                _conn.Dispose();
            }
        }
        public async Task<long> IncrementAsync(string key, long num = 1, int db = -1)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            var redis = Connect(db);
            var value = await redis.StringIncrementAsync(GetKeyForRedis(key), num);
            return value;
        }
        public async Task<bool> SortedAddAsync(string key, string member, double score, int db)
        {
            var redis =  Connect(db);
            return await redis.SortedSetAddAsync(GetKeyForRedis(key), member, score);
        }
        public async Task<Dictionary<string, double>> SortedRangeByScoreWithScoresAsync(string key, double min, double max, long skip,
            long take, Order order, int db)
        {
            var redis = Connect(db);
            var result = await redis.SortedSetRangeByScoreWithScoresAsync(GetKeyForRedis(key), min, max, Exclude.None, order, skip, take);
            var dic = new Dictionary<string, double>();
            foreach (var entry in result)
            {
                dic.Add(entry.Element, entry.Score);
            }
            return dic;
        }
        public string GetKeyForRedis(string key)
        {
            return $"{_instance}{key}";
        }

    }
}