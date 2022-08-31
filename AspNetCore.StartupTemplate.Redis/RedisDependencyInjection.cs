using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.StartupTemplate.Redis;

public static class RedisDependencyInjection
{
    public static IServiceCollection AddRedisManager(this IServiceCollection service)
    {
        service.AddSingleton<IRedisManager, RedisManager>();
        return service;
    }
}