using AspNetCore.StartupTemplate.CacheAsync.Interceptor;
using AspNetCore.StartupTemplate.CacheAsync.KeyGenerator;
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
        services.TryAddSingleton<IRedisClient>(sp =>
        {
            var options = sp.GetRequiredService<RedisOptions>();
            return new RedisClient(options.ConnectionString);
        });
        services.TryAddSingleton<ICacheKeyGenerator, DefaultCacheKeyGenerator>();
        return services;
    }
    public static WebApplication UseRedisCaching(this WebApplication app)
    {
        app.Use(async (httpContext, next) =>
        {
            CachingEnableAttribute.SetServiceProvider(httpContext.RequestServices);
            CacheClearAttribute.SetServiceProvider(httpContext.RequestServices);
            await next();
        });
        return app;
    }
}