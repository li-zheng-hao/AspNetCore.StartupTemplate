using AspNetCore.StartupTemplate.Redis;
using AspNetCore.StartUpTemplate.Configuration;
using AspNetCore.StartUpTemplate.Utility;
using Autofac;
using Newtonsoft.Json;
using RedLockNet;
using Rougamo.Context;

namespace AspNetCore.StartUpTemplate.Core.Cache;

/// <summary>
/// 任意接口的缓存
/// </summary
/// 
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class NeedCacheAttribute : Rougamo.MoAttribute
{
    /// <summary>
    /// 由于NeedCacheAttribute永远不会用using创建，因此无法通过IDisposable接口完成销毁，这里需要等待GC去清理这个对象
    /// </summary>
    ~NeedCacheAttribute()
    {
        _scope.Dispose();
    }
    /// <summary>
    /// 所有的接口缓存的前缀
    /// </summary>
    public const string METHOD_CACHE_PREFIX = "methodcache";
    public string CacheKey { get; set; }
    public IRedisManager _RedisManager { get; set; }
    private ILifetimeScope _scope { get; set; }
    private IRedLock _redLock { get; set; }
    public NeedCacheAttribute(string customKey = "")
    {
        CacheKey = customKey;
        _scope = IocHelper.GetNewILifeTimeScope();
        _RedisManager = _scope.Resolve<IRedisManager>();
    }
    public override void OnEntry(MethodContext context)
    {
        if (string.IsNullOrWhiteSpace(CacheKey))
            CacheKey = GenerateCacheKey(context);
        if (_RedisManager.Exists(CacheKey))
        {
            var resStr = _RedisManager.Get(CacheKey);
            var realResult = string.IsNullOrWhiteSpace(resStr)?null:JsonConvert.DeserializeObject(resStr, context.RealReturnType);
            context.ReplaceReturnValue(this, realResult);
        }
        else
        {
            // 并发量大需要处理缓存击穿问题 用互斥锁   
            // 超时默认10秒，如果没有获取到的话也无所谓 只要不会有大量请求跑数据库就行
            _redLock = _RedisManager.CreateLock("Lock" + CacheKey);
            //_redLock.IsAcquired 不需要这句
            // 获取锁后再判断一次，如果已经有了就不用去数据库再读了
            if (_RedisManager.Exists(CacheKey))
            {
                var resStr = _RedisManager.Get(CacheKey);
                var realResult =string.IsNullOrWhiteSpace(resStr)?null:  JsonConvert.DeserializeObject(resStr, context.RealReturnType);
                context.ReplaceReturnValue(this, realResult);
                _redLock.Dispose();
            }
        }
    }
    public override void OnExit(MethodContext context)
    {
        var returnVal = JsonConvert.SerializeObject(context.ReturnValue);
        if (string.IsNullOrWhiteSpace(returnVal) || context.ReturnValue == null)
        {
            // 防止缓存穿透
            _RedisManager.Set(CacheKey, "", TimeSpan.FromSeconds(AppSettingsConstVars.RedisExpireSec));
        }
        else
        {
            // 默认过期时间 加随机范围 防止雪崩
            _RedisManager.Set(CacheKey, returnVal
                , TimeSpan.FromSeconds(new Random().Next((int)Math.Floor(AppSettingsConstVars.RedisExpireSec * 0.8)
                    , (int)Math.Ceiling(AppSettingsConstVars.RedisExpireSec * 1.2))));
        }
        _redLock?.Dispose();

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


