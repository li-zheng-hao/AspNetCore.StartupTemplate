using FreeRedis;
using Microsoft.Extensions.DependencyInjection;
using MST.Infra.CacheProvider.KeyGenerator;
using Rougamo.Context;

namespace AspNetCore.StartupTemplate.CacheAsync.Interceptor;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class CacheClearAttribute : Rougamo.MoAttribute
{
    #region 需要的属性

    public bool IsBefore { get; set; }
    public string KeyPrefix { get; set; }

    #endregion


    private static IServiceProvider _serviceProvider;
    private  ICacheKeyGenerator _cacheKeyGenerator;
    public IRedisClient _redisClient { get; set; }
    public static void SetServiceProvider(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;


    /// <summary>
    /// 清除对应key前缀的缓存
    /// </summary>
    /// <param name="keyPrefix"></param>
    public CacheClearAttribute(string keyPrefix, bool isBefore = false)
    {
        IsBefore = isBefore;
        KeyPrefix = keyPrefix;
     
    }
    /// <summary>
    /// 清除对应key前缀的缓存
    /// </summary>
    /// <param name="keyPrefix"></param>
    public CacheClearAttribute(Type type,string methodName, bool isBefore = false)
    {
        IsBefore = isBefore;
        KeyPrefix = $"{type.Namespace}:{type.Name}:{methodName}";
     
    }

    public override void OnEntry(MethodContext context)
    {
        _redisClient = _serviceProvider.GetRequiredService<IRedisClient>();
        _cacheKeyGenerator = _serviceProvider.GetRequiredService<ICacheKeyGenerator>();
        // 换成在方法执行完后删除
        if (IsBefore)
            ClearCaching();
    }

    public override void OnExit(MethodContext context)
    {
    }
    public override void OnSuccess(MethodContext context)
    {
        if (!IsBefore)
            ClearCaching();
    }

    private void ClearCaching()
    {
        var res=_redisClient.Scan($"{KeyPrefix}*", 333, null).ToList();
        foreach (var keys in _redisClient.Scan($"{KeyPrefix}*", int.MaxValue, null))
        {
            Console.WriteLine(keys.ToString());
            _redisClient.UnLink(keys);

        }
    }
}