using AspNetCore.StartupTemplate.CustomScheduler.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.StartupTemplate.CustomScheduler;

public static class SchedulerDependencyInjection
{
    public static IServiceCollection AddScheduler(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ITaskManager,TaskManager>();
        serviceCollection.AddSingleton<SchedulerManager>();
        serviceCollection.AddTransient<DemoTask>();
        return serviceCollection;
    }
}