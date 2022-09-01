using Autofac;

namespace AspNetCore.StartUpTemplate.Core;

public class IocHelper
{
    public static ILifetimeScope container { get; set; }
    
    public static T Resolve<T>() where T : notnull
    {
        return container.Resolve<T>();
    }
    public static T ResolveWithName<T>(string name) where T : notnull
    {
        return container.ResolveNamed<T>(name);
    }
    
}