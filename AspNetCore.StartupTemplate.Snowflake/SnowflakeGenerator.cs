using FreeRedis;
using Yitter.IdGenerator;

namespace AspNetCore.StartupTemplate.Snowflake;

public class SnowflakeGenerator
{
    private readonly IRedisClient _redisClient;
    private readonly SnowflakeWorkIdManager _workIdManager;

    public SnowflakeGenerator(SnowflakeOption opt, SnowflakeWorkIdManager workIdManager)
    {
        _workIdManager = workIdManager;
        var options = new IdGeneratorOptions();
        options.WorkerId = SnowflakeWorkIdManager.WorkId;
        options.WorkerIdBitLength = opt.WorkerIdBitLength;
        options.SeqBitLength = opt.SeqBitLength;
        YitIdHelper.SetIdGenerator(options);
    }

    public long NextId()
    {
        return YitIdHelper.NextId();
    }
}