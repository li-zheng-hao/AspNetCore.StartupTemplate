using AspNetCore.StartUpTemplate.Configuration;
using AspNetCore.StartUpTemplate.Configuration.Option;
using FreeRedis;
using Microsoft.Extensions.Logging;

namespace AspNetCore.StartupTemplate.Snowflake;

public class SnowflakeWorkIdManager
{
    private readonly IRedisClient _redisClient;
    private readonly ILogger<SnowflakeWorkIdManager> _logger;
    private readonly SnowflakeOption _option;
    private readonly RedisOption _redisOption;
    private int CUR_WORK_ID { get; set; } = default!;
    public const string WORKID_COLLECTION_KEY = "snowflake_workid_set";
    public const string WORKID_CUR_INDEX = "snowflake_workid_cur_index";
    public static byte WorkId { get;  set; }
    public SnowflakeWorkIdManager(RedisOption redisOption,SnowflakeOption opt,IRedisClient redisClient,ILogger<SnowflakeWorkIdManager> logger)
    {
        _redisOption = redisOption;
        _option = opt;
        _logger = logger;
        _redisClient = redisClient;
        InitWorkId();
    }

    private int GetMaxWorkId()
    {
        return 1 << _option.WorkerIdBitLength;
    }
    private void InitWorkId()
    {
        if (_option.WorkId != null)
        {
            WorkId = _option.WorkId.Value;
            RefreshWorkId();
        }
        else
        {
            var curWorkId=_redisClient.Incr(WORKID_CUR_INDEX);
            if (curWorkId>GetMaxWorkId())
            {
                var workids=_redisClient.ZRangeByScore(WORKID_COLLECTION_KEY, 0, GetTimeStamp(DateTime.Now.AddSeconds(-15)), 0, 1);
                if (workids == null || workids.Length == 0)
                {
                    _logger.LogCritical("无法获取可用WorkId!");
                    throw new Exception("无法获取可用WorkId!");
                }
                WorkId = Convert.ToByte(workids[0]);
            }
            else
            {
                WorkId = (byte)curWorkId;
                RefreshWorkId();
            }
        }
       
    }
    
    public void RefreshWorkId()
    {
        _redisClient.ZAdd(WORKID_COLLECTION_KEY, (decimal)GetTimeStamp(), WorkId.ToString());
    }

    private long GetTimeStamp(DateTime? now=null)
    {
        var oldTime=new DateTime(2022,1,1);
        if (now != null)
            return now.Value.Ticks - oldTime.Ticks;
        else
            return DateTime.Now.Ticks - oldTime.Ticks;
    }
    public void UnRegisterWorkId()
    {
        // 这里必须新建一个才能在程序退出时删除，否则会提示连接池已经释放
        using RedisClient redisClient = new RedisClient(
            _redisOption.RedisConn+",poolsize=1",
            _redisOption.SentinelAdders.ToArray(),
            true //是否读写分离
        );
        redisClient.ZRem(WORKID_COLLECTION_KEY, WorkId.ToString());
        // _redisClient.ZRem(WORKID_COLLECTION_KEY, WorkId.ToString());
        
    }
   
    
    
}

