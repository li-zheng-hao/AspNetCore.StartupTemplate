using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AspNetCore.StartupTemplate.Snowflake
{
    public class SnowflakeBackgroundServices : IHostedService,IDisposable
    {
        private readonly SnowflakeGenerator _snowflakeGenerator;
        private readonly SnowflakeWorkIdManager _snowflakeWorkIdManager;
        private readonly ILogger<SnowflakeBackgroundServices> _logger;

        public SnowflakeBackgroundServices(ILogger<SnowflakeBackgroundServices> logger,SnowflakeWorkIdManager snowflakeWorkIdManager,SnowflakeGenerator snowflakeGenerator)
        {
            _logger = logger;
            _snowflakeWorkIdManager = snowflakeWorkIdManager;
            _snowflakeGenerator = snowflakeGenerator;
        }

        private void RefreshWorkId()
        {
            _snowflakeWorkIdManager.RefreshWorkId();
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                Task.Run(async () =>
                {
                    while (true)
                    {
                        RefreshWorkId();
                        //定时刷新机器id的存活状态
                        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                    }
                });



            }
        }
        
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _snowflakeWorkIdManager.UnRegisterWorkId();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _snowflakeWorkIdManager.Dispose();
        }
    }
}