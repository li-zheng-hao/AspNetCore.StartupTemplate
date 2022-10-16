using AspNetCore.StartUpTemplate.Configuration;
using AspNetCore.StartUpTemplate.Utility;
using AspNetCore.StartUpTemplate.Utility.Utils;
using Autofac;
using FreeRedis;
using Newtonsoft.Json;
using Rougamo.Context;

namespace AspNetCore.StartUpTemplate.Core.Cache;

/// <summary>
/// 任意接口的缓存
/// </summary
/// 
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class NeedCacheAttribute : Rougamo.MoAttribute
{
    private readonly GlobalConfig _config;

    /// <summary>
    /// 所有的接口缓存的前缀
    /// </summary>
    public const string METHOD_CACHE_PREFIX = "methodcache";
    public string CacheKey { get; set; }
    public IRedisClient _redisClient { get; set; }
    private RedisClient.LockController _lockController { get; set; }
    public NeedCacheAttribute(string customKey = "")
    {
        CacheKey = customKey;
        _redisClient = IocHelper.ResolveSingleton<IRedisClient>();
        _config = IocHelper.ResolveSingleton<GlobalConfig>();
    }
    public override void OnEntry(MethodContext context)
    {
        if (string.IsNullOrWhiteSpace(CacheKey))
            CacheKey = GenerateCacheKey(context);
        if (_redisClient.Exists(CacheKey))
        {
            var resStr = _redisClient.Get(CacheKey);
            var realResult = string.IsNullOrWhiteSpace(resStr)?null:JsonConvert.DeserializeObject(resStr, context.RealReturnType);
            context.ReplaceReturnValue(this, realResult);
        }
        else
        {
            // 并发量大需要处理缓存击穿问题 用互斥锁   
            // 超时默认10秒，如果没有获取到的话也无所谓 只要不会有大量请求跑数据库就行
             _lockController = _redisClient.Lock("Lock" + CacheKey,10);
            //_redLock.IsAcquired 不需要这句
            // 获取锁后再判断一次，如果已经有了就不用去数据库再读了
            if (_redisClient.Exists(CacheKey))
            {
                var resStr = _redisClient.Get(CacheKey);
                var realResult =string.IsNullOrWhiteSpace(resStr)?null:  JsonConvert.DeserializeObject(resStr, context.RealReturnType);
                context.ReplaceReturnValue(this, realResult);
                _lockController.Dispose();
            }
        }
    }
    public override void OnExit(MethodContext context)
    {
        var returnVal = JsonConvert.SerializeObject(context.ReturnValue);
        if (string.IsNullOrWhiteSpace(returnVal) || context.ReturnValue == null)
        {
            // 防止缓存穿透
            _redisClient.Set(CacheKey, "", TimeSpan.FromSeconds(new Random().Next((int)Math.Floor(_config.Redis.RedisCacheExpireSec * 0.8)
                , (int)Math.Ceiling(_config.Redis.RedisCacheExpireSec * 1.2))));
        }
        else
        {
            // 默认过期时间 加随机范围 防止雪崩
            _redisClient.Set(CacheKey, returnVal
                , TimeSpan.FromSeconds(new Random().Next((int)Math.Floor(_config.Redis.RedisCacheExpireSec * 0.8)
                    , (int)Math.Ceiling(_config.Redis.RedisCacheExpireSec * 1.2))));
        }
        _lockController?.Dispose();

    }

    private string GenerateCacheKey(MethodContext context)
    {
        string className = context.TargetType.Name;
        string methodName = context.Method.Name;
        List<object> methodArguments = context.Arguments.ToList();
        string param = string.Empty;
        if (methodArguments.Count > 0)
        {
            string serializeString = JsonConvert.SerializeObject(methodArguments, Formatting.Indented, new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Ignore
            });
            param = ":" + EncryptUtil.Encrypt(serializeString);
        }
        return string.Concat($"{METHOD_CACHE_PREFIX}:{className}:{methodName}", param);
    }



}


