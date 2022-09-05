using AspNetCore.StartUpTemplate.Configuration;
using AspNetCore.StartupTemplate.Redis;
using AspNetCore.StartUpTemplate.Utility;
using Autofac;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Rougamo.Context;
using Serilog;

namespace AspNetCore.StartUpTemplate.Core.Cache;

/// <summary>
/// 任意接口的缓存
/// </summary>
public class ClearCacheAttribute:Rougamo.MoAttribute
{
    /// <summary>
    /// 由于NeedCacheAttribute永远不会用using创建，因此无法通过IDisposable接口完成销毁，这里需要等待GC去清理这个对象
    /// </summary>
    ~ClearCacheAttribute()
    {
        _scope.Dispose();
    }
    /// <summary>
    /// 所有的接口缓存的前缀
    /// </summary>
    public const string METHOD_CACHE_PREFIX = "methodcache";
    public IRedisManager _RedisManager { get; set; }
    private ILifetimeScope _scope { get; set; }

    private bool _isFromOtherClass { get; set; } = false;
    private Type? _classInfo { get; set; }
    private List<string> _methodName { get; set; } = new List<string>();
    /// <summary>
    /// 清除本特性标记的方法同类下的其他方法缓存,不写的话默认清除所有
    /// </summary>
    /// <param name="methodNames"></param>
    public ClearCacheAttribute(params string[] methodNames)
    {
        _scope=IocHelper.GetNewILifeTimeScope();
        _RedisManager= _scope.Resolve<IRedisManager>();
        _methodName.AddRange(methodNames);
    }
    /// <summary>
    /// 清除其他类下的方法缓存
    /// </summary>
    /// <param name="isFromOtherClass"></param>
    /// <param name="classInfo"></param>
    /// <param name="methodNames"></param>
    public ClearCacheAttribute(bool isFromOtherClass,Type classInfo,params string[] methodNames)
    {
        _scope=IocHelper.GetNewILifeTimeScope();
        _RedisManager= _scope.Resolve<IRedisManager>();
        _isFromOtherClass = isFromOtherClass;
        _classInfo = classInfo;
        _methodName.AddRange(methodNames);
    }
    public override void OnEntry(MethodContext context)
    {
        // todo 根据构造函数传参的不同选择不同的清除方式
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


