using AspNetCore.StartUpTemplate.Utility;
using Autofac;
using Newtonsoft.Json;
using Rougamo.Context;
using System.Reflection;
using FreeRedis;

namespace AspNetCore.StartUpTemplate.Core.Cache;

/// <summary>
/// 清除缓存
/// </summary>'
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class ClearCacheAttribute : Rougamo.MoAttribute
{
    /// <summary>
    /// 所有的接口缓存的前缀
    /// </summary>
    public const string METHOD_CACHE_PREFIX = "methodcache";

    public IRedisClient _redisClient { get; set; }
    private ILifetimeScope _scope { get; set; }

    private Type? _classInfo { get; set; }
    private List<string> _methodNames { get; set; } = new List<string>();

    private bool _clearByAtribute { get; set; } = false;

    /// <summary>
    /// 清除特定类下的方法缓存
    /// </summary>
    /// <param name="classInfo">要清除的类</param>
    /// <param name="methodNames">方法名</param>
    public ClearCacheAttribute(Type classInfo, params string[] methodNames)
    {
        if (classInfo == null)
            throw new ArgumentException("参数有误");
        _redisClient = IocHelper.ResolveSingleton<IRedisClient>();
        _classInfo = classInfo;
        _methodNames.AddRange(methodNames);
    }

    /// <summary>
    /// 清除类中所有标记了NeedClearCache特性的方法
    /// </summary>
    /// <param name="classInfo"></param>
    /// <param name="clearByAttr"></param>
    public ClearCacheAttribute(Type classInfo, bool clearByAtribute)
    {
        if (classInfo == null)
            throw new ArgumentException("参数有误");
        _redisClient = IocHelper.ResolveSingleton<IRedisClient>();
        _classInfo = classInfo;
        _clearByAtribute = clearByAtribute;
    }

    public override void OnEntry(MethodContext context)
    {
        // 换成在方法执行完后删除
    }

    public override void OnExit(MethodContext context)
    {
        // 根据特性清除
        if (_clearByAtribute)
        {
            var methods = _classInfo.GetMethods()
                .Where(it => it.GetCustomAttributes<NeedClearCacheAttribute>().FirstOrDefault() != null).ToList();
            methods.ForEach(it =>
            {
                var keys = _redisClient.Keys($"{METHOD_CACHE_PREFIX}:{_classInfo.Name}:{it.Name}*");
                if (keys != null && keys.Length > 0)
                    _redisClient.Del(keys);
            });
        }
        // 根据方法名称清除
        else
        {
            // 清除本类下所有的缓存
            if (_methodNames.Count == 0)
            {
                var keys = _redisClient.Keys($"{METHOD_CACHE_PREFIX}:{_classInfo.Name}*");
                if (keys != null && keys.Length > 0)
                    _redisClient.Del(keys);
            }
            // 根据方法名清除
            else
            {
                _methodNames.ForEach(methodName =>
                {
                    var keys = _redisClient.Keys($"{METHOD_CACHE_PREFIX}:{_classInfo.Name}:{methodName}*");
                    if (keys != null && keys.Length > 0)
                        _redisClient.Del(keys);
                });
            }
        }
    }
}