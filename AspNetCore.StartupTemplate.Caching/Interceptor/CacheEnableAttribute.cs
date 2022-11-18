using AspNetCore.StartupTemplate.CacheAsync.Configuration;
using FreeRedis;
using Microsoft.Extensions.DependencyInjection;
using MST.Infra.CacheProvider.KeyGenerator;
using Newtonsoft.Json;
using Rougamo;
using Rougamo.Context;

namespace MST.Infra.CacheProvider.Interceptor;

[AttributeUsage(AttributeTargets.Method,AllowMultiple = false,Inherited = false)]
public class CachingEnableAttribute:MoAttribute
{
    #region 需要配置的属性
    private string _cacheKey { get; set; }
    /// <summary>
    /// 过期时间 秒
    /// </summary>
    public double? ExpireSec { get; set; }

    #endregion    

    private static IServiceProvider _serviceProvider;
    private  IRedisClient _redisClient;
    private  ICacheKeyGenerator _cachingKeyGenerator;
    private RedisClient.LockController _lockController;
    private  CacheOptions _cacheOption;
    /// <summary>
    /// 这里传进来的是非root级别的serviceprovider
    /// </summary>
    /// <param name="serviceProvider"></param>
    public static void SetServiceProvider(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;
    /// <summary>
    /// 使用Redis进行缓存 不设置Key的话则默认为如下格式：
    /// namespace:className:methodName:参数Json序列化md5
    /// </summary>
    public CachingEnableAttribute(string customKey = "")
    {
        _cacheKey = customKey;
    }


    public override void OnEntry(MethodContext context)
    {
        _redisClient = _serviceProvider.GetRequiredService<IRedisClient>();
        _cacheOption = _serviceProvider.GetRequiredService<CacheOptions>();
        _cachingKeyGenerator =_serviceProvider.GetRequiredService<ICacheKeyGenerator>();
        if (string.IsNullOrWhiteSpace(_cacheKey))
            _cacheKey = _cachingKeyGenerator.GeneratorKey(context);
        if (_redisClient.Exists(_cacheKey))
        {
            var resStr = _redisClient.Get(_cacheKey);
            var realResult = string.IsNullOrWhiteSpace(resStr)?null:JsonConvert.DeserializeObject(resStr, context.RealReturnType);
            context.ReplaceReturnValue(this, realResult);
        }
        else
        {
            // 并发量大需要处理缓存击穿问题 用互斥锁   
            // 超时默认10秒，如果没有获取到的话也无所谓 只要不会有大量请求跑数据库就行
            _lockController = _redisClient.Lock("Lock" + _cacheKey,10);
            // 获取锁后再判断一次，如果已经有了就不用去数据库再读了
            if (_redisClient.Exists(_cacheKey))
            {
                var resStr = _redisClient.Get(_cacheKey);
                var realResult =string.IsNullOrWhiteSpace(resStr)?null:  JsonConvert.DeserializeObject(resStr, context.RealReturnType);
                context.ReplaceReturnValue(this, realResult);
                _lockController?.Dispose();
            }
        }
    }

    public override void OnExit(MethodContext context)
    {
        _lockController?.Dispose();
    }
    public override void OnSuccess(MethodContext context)
    {
        if (typeof(Task).IsAssignableFrom(context.RealReturnType))
            ((Task)context.ReturnValue).ContinueWith(t => _OnExit());
        else _OnExit();

        void _OnExit()
        {
            try
            {
                var expireSec = ExpireSec ?? _cacheOption.RedisCacheExpireSec;
                var returnVal = JsonConvert.SerializeObject(context.ReturnValue);
                if (string.IsNullOrWhiteSpace(returnVal) || context.ReturnValue == null)
                {
                    // 防止缓存穿透
                    _redisClient.Set(_cacheKey, "", TimeSpan.FromSeconds(new Random().Next((int)Math.Floor(expireSec* 0.8)
                        , (int)Math.Ceiling(expireSec * 1.2))));
                }
                else
                {
                    // 默认过期时间 加随机范围 防止雪崩
                    _redisClient.Set(_cacheKey, returnVal,
                    TimeSpan.FromSeconds(new Random().Next((int)Math.Floor(expireSec * 0.8)
                        , (int)Math.Ceiling(expireSec * 1.2))));
                }
            }
            finally
            {
            }
        }
    }


  
}