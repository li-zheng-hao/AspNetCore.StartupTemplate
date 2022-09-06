using AspNetCore.StartupTemplate.Redis;
using AspNetCore.StartUpTemplate.Utility;
using Autofac;
using Newtonsoft.Json;
using Rougamo.Context;
using System.Reflection;

namespace AspNetCore.StartUpTemplate.Core.Cache;

/// <summary>
/// 清除缓存
/// </summary>'
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class ClearCacheAttribute : Rougamo.MoAttribute
{
    /// <summary>
    /// 由于ClearCacheAttribute永远不会用using创建，因此无法通过IDisposable接口完成销毁，这里需要等待GC去清理这个对象
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
        _scope = IocHelper.GetNewILifeTimeScope();
        _RedisManager = _scope.Resolve<IRedisManager>();
        _classInfo = classInfo;
        _methodNames.AddRange(methodNames);
    }
    /// <summary>
    /// 清除类中所有标记了NeedClearCache特性的方法
    /// </summary>
    /// <param name="classInfo"></param>
    /// <param name="clearByAttr"></param>
    public ClearCacheAttribute(Type classInfo,bool clearByAtribute)
    {
        if (classInfo == null)
            throw new ArgumentException("参数有误");
        _scope = IocHelper.GetNewILifeTimeScope();
        _RedisManager = _scope.Resolve<IRedisManager>();
        _classInfo = classInfo;
        _clearByAtribute = clearByAtribute;
    }
    public override void OnEntry(MethodContext context)
    {
        // 根据特性清除
        if (_clearByAtribute)
        {
            var methods = _classInfo.GetMethods().Where(it => it.GetCustomAttributes<NeedClearCacheAttribute>().FirstOrDefault() != null).ToList();
            methods.ForEach(it =>
            {
                _RedisManager.RemoveMultiKey($"{METHOD_CACHE_PREFIX}:{_classInfo.Name}:{it.Name}*");
            });
        }
        // 根据方法名称清除
        else
        {
            // 清除本类下所有的缓存
            if (_methodNames.Count == 0)
            {
                _RedisManager.RemoveMultiKey($"{METHOD_CACHE_PREFIX}:{_classInfo.Name}*");
            }
            // 根据方法名清除
            else
            {
                _methodNames.ForEach(methodName =>
                {
                    _RedisManager.RemoveMultiKey($"{METHOD_CACHE_PREFIX}:{_classInfo.Name}:{methodName}*");
                });
            }

        }
    }


}


