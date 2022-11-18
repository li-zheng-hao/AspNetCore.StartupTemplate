using AspNetCore.CacheOutput;
using AspNetCore.CacheOutput.Redis;
using AspNetCore.StartUpTemplate.Configuration.Option;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;

namespace AspNetCore.StartUpTemplate.Core.ServiceRegister;

public static partial  class ServiceRegister
{
    public static IServiceCollection AddCustomRedisCacheOutput(
        this IServiceCollection services,IConfiguration configuration)
    {
        var redisOption = configuration.GetSection("Redis").Get<RedisOption>();
        if (services == null)
            throw new ArgumentNullException(nameof(services));
        services.TryAdd(ServiceDescriptor.Singleton<CacheKeyGeneratorFactory, CacheKeyGeneratorFactory>());
        services.TryAdd(ServiceDescriptor.Singleton<ICacheKeyGenerator, DefaultCacheKeyGenerator>());
        services.TryAdd(ServiceDescriptor.Singleton<IApiCacheOutput, StackExchangeRedisCacheOutputProvider>());
        ConfigurationOptions options = new ConfigurationOptions();
        options.Password = redisOption.Password;
        options.CommandMap = CommandMap.Sentinel;
        foreach (var addr in redisOption.SentinelAdders)
        {
            var ipPort = addr.Split(':');
            options.EndPoints.Add(ipPort[0], Convert.ToInt32(ipPort[1]));
        }
        options.TieBreaker = ""; //这行在sentinel模式必须加上
        options.DefaultVersion = new Version(3, 0);
        options.AllowAdmin = true;
        var masterConfig = new ConfigurationOptions
        {
            CommandMap = CommandMap.Default,
            ServiceName = redisOption.ServiceName,
            Password = redisOption.Password,
            AllowAdmin = true
        };
        services.TryAdd(
            ServiceDescriptor.Singleton<IConnectionMultiplexer>(
                (IConnectionMultiplexer)ConnectionMultiplexer.Connect(options)));
        services.TryAdd(ServiceDescriptor.Transient<IDatabase>((Func<IServiceProvider, IDatabase>)(e =>
            ((ConnectionMultiplexer)e.GetRequiredService<IConnectionMultiplexer>())
            .GetSentinelMasterConnection(masterConfig).GetDatabase())));
        return services;
    }
}