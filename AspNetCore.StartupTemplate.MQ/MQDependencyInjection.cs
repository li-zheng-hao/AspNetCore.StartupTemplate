using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.StartupTemplate.MQ;

/// <summary>
/// 依赖注入MQ 全局实例
/// </summary>
public static class MQDependencyInjection
{
    public static IServiceCollection AddRabbitMQManager(this IServiceCollection service)
    {
        service.AddSingleton<IMQManager,MQManager>();
        return service;
    }
}