using AspNetCore.StartupTemplate.CacheAsync.Interceptor;
using AspNetCore.StartupTemplate.CacheAsync.KeyGenerator;
using AspNetCore.StartUpTemplate.Configuration.Option;
using FreeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MST.Infra.CacheProvider.Interceptor;
using MST.Infra.CacheProvider.KeyGenerator;
using Quickwire;

namespace AspNetCore.StartupTemplate.CacheAsync.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddRedisCaching(this IServiceCollection services)
    {
        services.ScanCurrentAssembly();
        services.TryAddSingleton<ICacheKeyGenerator, DefaultCacheKeyGenerator>();
        return services;
    }
}