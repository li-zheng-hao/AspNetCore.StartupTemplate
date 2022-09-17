using FreeRedis;
using IdGen;

namespace AspNetCore.StartupTemplate.Snowflake;

public class SnowflakeGenerator
{
    private readonly IRedisClient _redisClient;
    private readonly SnowflakeWorkIdManager _workIdManager;
    private readonly IdGenerator _generator;

    public SnowflakeGenerator(SnowflakeOption opt, SnowflakeWorkIdManager workIdManager)
    {
        _workIdManager = workIdManager;
        var epoch = new DateTime(2020, 1,1);
         
        var structure = new IdStructure(46, opt.WorkerIdBitLength, opt.SeqBitLength);
        var options = new IdGeneratorOptions(structure, new DefaultTimeSource(epoch));
        _generator = new IdGenerator(SnowflakeWorkIdManager.WorkId,options);
        // YitIdHelper.SetIdGenerator(options);
    }

    public long NextId()
    {
        return _generator.CreateId();
    }
}