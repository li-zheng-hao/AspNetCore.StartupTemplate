using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.StartupTemplate.Job;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCustomHangfireService(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHangfire(config=>config.UseMemoryStorage());
        serviceCollection.AddHangfireServer(options =>
        {
    
        });
        return serviceCollection;
    }
}