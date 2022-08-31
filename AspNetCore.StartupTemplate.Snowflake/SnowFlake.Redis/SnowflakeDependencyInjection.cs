using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.StartupTemplate.Snowflake.SnowFlake.Redis
{
    public static class SnowflakeDependencyInjection
    {
        public static IServiceCollection AddSnowflakeWithRedis(this IServiceCollection service, Action<RedisOption> option)
        {
            service.Configure(option);
            service.AddSingleton<ISnowflakeIdMaker, SnowflakeIdMaker>();
            service.AddSingleton<IRedisClient, RedisClient>();
            service.AddSingleton<IDistributedSupport, DistributedSupportWithRedis>();
            service.AddHostedService<SnowflakeBackgroundServices>();
            return service;
        }
    }
}