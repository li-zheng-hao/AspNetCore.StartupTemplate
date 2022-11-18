using System.Formats.Asn1;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.StartUpTemplate.Core;
/// <summary>
/// root service provider
/// </summary>
public static class ServiceProviderLocator
{ 
    public static IServiceProvider RootServiceProvider { get; set; }
    public static T GetService<T>()
    {
        return (T)RootServiceProvider.GetService(typeof(T));
    }
    public static T GetRequiredService<T>()
    {
        return (T)RootServiceProvider.GetRequiredService(typeof(T));
    }
}