using AspNetCore.StartUpTemplate.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.StartupTemplate.Snowflake.SnowFlake.Redis
{
    public static class SnowflakeDependencyInjection
    {
        public static IServiceCollection AddSnowflakeWithRedis(this IServiceCollection service, Action<RedisOption>? option=null)
        {
            if (option == null)
            {
                option = opt =>
                {
                    opt.InstanceName = "snowflake:";
                    opt.ConnectionString = AppSettingsConstVars.RedisConn;
                    opt.WorkIdLength = 9; // 9位支持512个工作节点
                    opt.RefreshAliveInterval = TimeSpan.FromMinutes(4);
                    opt.StartTimeStamp = DateTime.Parse("2000-01-01");
                };
            }
            service.Configure(option);
            service.AddSingleton<ISnowflakeIdMaker, SnowflakeIdMaker>();
            service.AddSingleton<IRedisClient, RedisClient>();
            service.AddSingleton<IDistributedSupport, DistributedSupportWithRedis>();
            service.AddHostedService<SnowflakeBackgroundServices>();
            return service;
        }
    }
}