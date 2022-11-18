using AspNetCore.StartUpTemplate.Configuration.Option;
using FreeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.StartUpTemplate.Core.ServiceRegister;

public static partial  class ServiceRegister
{
    public static IServiceCollection AddFreeRedis(this IServiceCollection serviceCollection,IConfiguration configuration)
    {
        var redisOption = configuration.GetSection("Redis").Get<RedisOption>();
        RedisClient redisClient = new RedisClient(
            redisOption.RedisConn,
            redisOption.SentinelAdders.ToArray(),
            true //是否读写分离
        );
        serviceCollection.AddSingleton<IRedisClient>(redisClient);
        return serviceCollection;
    }
}