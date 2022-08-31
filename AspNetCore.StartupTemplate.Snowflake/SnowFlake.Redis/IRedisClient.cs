using StackExchange.Redis;

namespace AspNetCore.StartupTemplate.Snowflake.SnowFlake.Redis
{
    public interface IRedisClient: IDisposable
    {
        Task<long> IncrementAsync(string key, long num = 1, int db = -1);
        string GetKeyForRedis(string key);

        Task<Dictionary<string, double>> SortedRangeByScoreWithScoresAsync(string key, double min = double.MinValue,
            double max = double.MaxValue,
            long skip = 0,
            long take = -1, Order order = Order.Descending, int db = -1);

        Task<bool> SortedAddAsync(string key, string member, double score, int db = -1);
    }
}