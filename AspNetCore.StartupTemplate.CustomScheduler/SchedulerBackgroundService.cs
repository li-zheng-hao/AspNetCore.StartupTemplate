using Microsoft.Extensions.Hosting;

namespace AspNetCore.StartupTemplate.CustomScheduler;
/// <summary>
/// TODO 定时任务服务 
/// </summary>
public class SchedulerBackgroundService:IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}