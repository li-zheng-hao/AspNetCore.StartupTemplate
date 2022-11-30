using AspNetCore.StartUpTemplate.Configuration.Option;
using FreeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.StartUpTemplate.Core.ServiceRegister;

public static partial class ServiceRegister
{
    public static IServiceCollection AddFreeRedis(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var redisOption = configuration.GetSection("Redis").Get<RedisOption>();
        serviceCollection.AddSingleton<IRedisClient>(sp =>
            new RedisClient(
                redisOption.RedisConn,
                redisOption.SentinelAdders.ToArray(),
                true //是否读写分离
            )
        );
        return serviceCollection;
    }
}