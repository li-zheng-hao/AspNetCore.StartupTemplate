using Autofac;

namespace AspNetCore.StartUpTemplate.Core;
/// <summary>
/// 本方法解析出来的都是在一个总的ILifetimeScope生命周期内，并且是全局的生命周期，所以只能通过这个类获取Singleton类型的对象，
/// 或者在方法一开始就调用LifeTimeScope获取嵌套的生命周期，否则lifetime拥有了AddTransient后的对象，永远也不会销毁
/// 这样解析出来的对象autofac不会主动调用dispose，除非程序退出，切记！否则将产生一些奇怪的bug
/// </summary>
public class IocHelper
{
    public static ILifetimeScope GlobalLifetimeScope { get; set; }
    /// <summary>
    /// 切记只能用于单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T Resolve<T>() where T : notnull
    {
        return GlobalLifetimeScope.Resolve<T>();
    }

    public static ILifetimeScope GetILifeTimeScope()
    {
        return GlobalLifetimeScope.BeginLifetimeScope();
    }
    
}