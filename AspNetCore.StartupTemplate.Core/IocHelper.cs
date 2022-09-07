using System.Formats.Asn1;
using Autofac;

namespace AspNetCore.StartUpTemplate.Core;
public class IocHelper
{
    private static ILifetimeScope _globalLifetimeScope { get; set; }
    /// <summary>
    /// 在根生命周期范围内创建嵌套的生命周期范围
    /// 重点：必须using或者手动dispose！
    /// </summary>
    /// <returns></returns>
    public static ILifetimeScope GetNewILifeTimeScope()
    {
        var scope= _globalLifetimeScope.BeginLifetimeScope();
        return scope;
    }
    /// <summary>
    /// 只能用于解析静态类，否则即内存泄漏
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ResolveSingleton<T>()
    {
        return _globalLifetimeScope.Resolve<T>();
    }
    public static void SetGlobalLifeTimeScope(ILifetimeScope scope)
    {
        _globalLifetimeScope = scope;
    }
    
}